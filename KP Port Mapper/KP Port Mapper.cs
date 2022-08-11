using Open.Nat;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KP_Port_Mapper;
public partial class FormKPPortMapper : Form
{
    public FormKPPortMapper()
    {
        SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        UpdateStyles();
        InitializeComponent();
    }

    private readonly NatDiscoverer discoverer = new();
    private string extIP;
    private readonly string intIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
    private NatDevice device;
    private async void Form1_Load(object sender, EventArgs e)
    {
        device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, new CancellationTokenSource(10000));
        extIP = (await device.GetExternalIPAsync()).ToString();
        dataGridPortsView.Columns.AddRange(new[] {
            new DataGridViewTextBoxColumn { DataPropertyName = "Protocol", Name = "Type", Width = 42, ReadOnly = true },
            new DataGridViewTextBoxColumn { DataPropertyName = "PrivatePort", Name = "Private", Width = 70, ReadOnly = true },
            new DataGridViewTextBoxColumn { DataPropertyName = "PublicPort", Name = "Public", Width = 60, ReadOnly = true },
            new DataGridViewTextBoxColumn { DataPropertyName = "Description", Name = "Description", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true }
            });
        DataGridMethods.AlignColumns(dataGridPortsView.Columns);
        dataGridSuggestionView.Columns.AddRange(new[] {
            new DataGridViewTextBoxColumn { DataPropertyName = "Port", Name = "Port", Width = 56, ReadOnly = true },
            new DataGridViewTextBoxColumn { DataPropertyName = "Protocol", Name = "Type", Width = 42, ReadOnly = true },
            new DataGridViewTextBoxColumn { DataPropertyName = "Process", Name = "Process", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true },
            new DataGridViewTextBoxColumn { DataPropertyName = "Title", Name = "Title", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true }
            });
        DataGridMethods.AlignColumns(dataGridSuggestionView.Columns);
        labelPublicIP.Text = $"IP Address is: {extIP}";
        labelPublicIP.DoubleClick += (s, e) => { ShowNotif($"Copied IP {extIP} to clipboard.", 3000); Clipboard.SetText(extIP); };
        labelPrivateIP.Text = $"Private IP: {intIP}";
        labelPrivateIP.DoubleClick += (s, e) => { ShowNotif($"Copied IP {intIP} to clipboard.", 3000); Clipboard.SetText(intIP); };
        BackgroundWorker worker = new();
        worker.DoWork += async (s, e) =>
        {
            while (true)
            {
                DataGridMethods.GenerateRows(dataGridPortsView, device);
                DataGridMethods.GetSuggestedPorts(dataGridSuggestionView);
                await Task.Delay(2500);
            }
        };
        worker.RunWorkerAsync();
    }

    private void DataGridPortsView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        DataGridViewRow dgvRow = (sender as DataGridView).Rows[e.RowIndex];
        IPDataObject ip = dgvRow.DataBoundItem as IPDataObject;
        string text = e.ColumnIndex == 1 ? $"{intIP}:{ip.PrivatePort}" : $"{extIP}:{ip.PublicPort}";
        ShowNotif($"Copied IP {text} to clipboard.", 3000);
        Clipboard.SetText(text);
    }
    private bool notifShown = false;
    private void ShowNotif(string text, int duration, bool error = false)
    {
        if (notifShown)
        {
            Controls[0].Text = text;
            return;
        }
        int width = (int)(Width * 0.75);
        Label notif = new()
        {
            Width = width,
            Location = new Point((Width - width) / 2, 15),
            BackColor = error ? Color.Red : Color.LimeGreen,
            Text = text
        };
        Controls.Add(notif);
        Controls.SetChildIndex(notif, 0);
        notifShown = true;
        Task.Run(() =>
        {
            int i = 0;
            for (; i < 24; i++)
            {
                Invoke(new MethodInvoker(() => notif.Height = i));
                Thread.Sleep(5);
            }
            Thread.Sleep(duration);
            for (; i >= 0; i--)
            {
                Invoke(new MethodInvoker(() => notif.Height = i));
                Thread.Sleep(5);
            }
            Invoke(new MethodInvoker(() =>
            {
                notifShown = false;
                Controls.Remove(notif);
                notif.Dispose();
            }));
        });
    }

    private void DataGridSuggestionView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        DataGridViewRow dgvRow = (sender as DataGridView).Rows[e.RowIndex];
        PortSuggestionDataObject ps = dgvRow.DataBoundItem as PortSuggestionDataObject;
        textboxPrivatePortMin.Text = ps.Port;
        textBoxDescription.Text = ps.Process;
        checkBoxTCP.Checked = ps.Protocol != "UDP";
        checkBoxUDP.Checked = ps.Protocol != "TCP";
    }

    private async void DataGridPortsView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
        IPDataObject port = e.Row.DataBoundItem as IPDataObject;
        if (port.Protocol != "DYN")
        {
            await device.DeletePortMapAsync(new Mapping(Enum.Parse<Protocol>(port.Protocol, true), port.PrivatePort, port.PublicPort, textBoxDescription.Text));
            return;
        }
        await device.DeletePortMapAsync(new Mapping(Protocol.Tcp, port.PrivatePort, port.PublicPort, textBoxDescription.Text));
        await device.DeletePortMapAsync(new Mapping(Protocol.Udp, port.PrivatePort, port.PublicPort, textBoxDescription.Text));
    }

    private void Textbox_KeyDown(object sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Back:
            case Keys.End:
            case Keys.Home:
            case var _ when e.KeyCode >= Keys.Left && e.KeyCode <= Keys.Down:
            case var _ when e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9:
            case var _ when e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9:
            case var _ when e.KeyCode is Keys.A or Keys.C or Keys.V or Keys.X && e.Control:
                return;
        }
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    private void Textbox_TextChanged(object sender, EventArgs e)
    {
        TextBox tb = sender as TextBox;
        if (tb.PasswordChar == ' ')
        {
            tb.PasswordChar = '\0';
            return;
        }
        if (int.Parse("0" + tb.Text) > ushort.MaxValue)
        {
            int s = tb.SelectionStart;
            ChangePort(tb, ushort.MaxValue.ToString());
            tb.SelectionStart = s;
        }
        int span = Math.Abs(int.Parse("0" + textboxPrivatePortMin.Text) - int.Parse("0" + textboxPrivatePortMax.Text));
        switch (tb.Name)
        {
            case "textboxPrivatePortMin":
                foreach (TextBox t in new TextBox[] { textboxPrivatePortMax, textboxPublicPortMin, textboxPublicPortMax })
                    ChangePort(t, tb.Text);
                return;
            case "textboxPrivatePortMax":
            case "textboxPublicPortMin":
                ChangePort(textboxPublicPortMax, (int.Parse("0" + textboxPublicPortMin.Text) + span).ToString());
                break;
            case "textboxPublicPortMax":
                ChangePort(textboxPublicPortMin, (int.Parse("0" + textboxPublicPortMax.Text) - span).ToString());
                return;
        }
    }
    private static void ChangePort(TextBox textBox, string text)
    {
        if (textBox.Text == text)
            return;
        textBox.PasswordChar = ' ';
        textBox.Text = text;
    }

    private void DataGridPortsView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        DataGridView dgv = sender as DataGridView;
        if (e.Value is not "TCP" and not "UDP" and not "DYN")
            return;
        foreach (DataGridViewCell item in dgv.Rows[e.RowIndex].Cells)
        {
            Color color = e.Value.ToString() switch
            {
                "TCP" => Color.DarkTurquoise,
                "UDP" => Color.LimeGreen,
                "DYN" => Color.MediumSeaGreen,
                _ => throw new NotImplementedException()
            };
            item.Style.BackColor = color;
            item.Style.SelectionBackColor = color;
        }
    }
    private async void ButtonOpenPort_Click(object sender, EventArgs e)
    {
        int startPort = Math.Min(int.Parse("0" + textboxPrivatePortMin.Text), int.Parse("0" + textboxPrivatePortMax.Text));
        if (startPort == 0)
            return;

        int startPublicPort = int.Parse("0" + textboxPublicPortMin.Text);
        if (startPublicPort == 0)
            return;

        int span = Math.Abs(startPort - Math.Max(int.Parse("0" + textboxPrivatePortMin.Text), int.Parse("0" + textboxPrivatePortMax.Text)));

        int realCount = span * (Convert.ToInt32(checkBoxTCP.Checked) + Convert.ToInt32(checkBoxUDP.Checked));
        if (realCount > 10 && MessageBox.Show($"Are you sure you want to open {realCount} ports?") != DialogResult.Yes)
            return;

        for (int i = 0; i <= span; i++)
        {
            try
            {
                if (checkBoxTCP.Checked)
                {
                    if (await device.GetSpecificMappingAsync(Protocol.Tcp, startPort + i) != null)
                    {
                        ShowNotif($"Private TCP port: {startPublicPort + i} already mapped!", 3000, true);
                        continue;
                    }
                    await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, startPort + i, startPublicPort + i, int.MaxValue - 1, textBoxDescription.Text != "" ? textBoxDescription.Text : " "));
                }
                if (checkBoxUDP.Checked)
                {
                    if (await device.GetSpecificMappingAsync(Protocol.Udp, startPort + i) != null)
                    {
                        ShowNotif($"Private UDP port: {startPublicPort + i} already mapped!", 3000, true);
                        continue;
                    }
                    await device.CreatePortMapAsync(new Mapping(Protocol.Udp, startPort + i, startPublicPort + i, int.MaxValue - 1, textBoxDescription.Text != "" ? textBoxDescription.Text : " "));
                }
            }
            catch
            {
                ShowNotif($"Public port: {startPublicPort + i} already mapped!", 3000, true);
            }
        }
        DataGridMethods.GenerateRows(dataGridPortsView, device);
    }

    private void ButtonSuggestionRefresh_Click(object sender, EventArgs e)
    {
        DataGridMethods.GenerateRows(dataGridPortsView, device);
        DataGridMethods.GetSuggestedPorts(dataGridSuggestionView);
    }
}
