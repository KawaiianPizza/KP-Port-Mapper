using Mono.Nat;
using Mono.Nat.Logging;
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
        NatUtility.DeviceFound += DeviceFound;
        NatUtility.StartDiscovery();
        SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        UpdateStyles();
        InitializeComponent();
    }

    private string extIP;
    private readonly string intIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
    private INatDevice device;
    private async void DeviceFound(object sender, DeviceEventArgs args)
    {
        try
        {
            if (args.Device.NatProtocol == NatProtocol.Upnp)
                device = args.Device;
            device ??= args.Device;

            extIP = (await device.GetExternalIPAsync()).ToString();
            labelPublicIP.Text = $"IP Address is: {extIP}";
        }
        finally
        {
            if (device != null)
                DataGridMethods.GenerateRows(dataGridPortsView, device);
        }
    }
    private async void Form1_Load(object sender, EventArgs e)
    {
        DataGridMethods.ConfigureAndAlignColumns(dataGridPortsView,
            ("Protocol", "Type", 42),
            ("PrivatePort", "Private", 70),
            ("PublicPort", "Public", 60),
            ("Description", "Description", 0) // 0 for AutoSizeMode.Fill
        );
        DataGridMethods.ConfigureAndAlignColumns(dataGridSuggestionView,
            ("Port", "Port", 56),
            ("Protocol", "Type", 42),
            ("Process", "Process", 0),
            ("Title", "Title", 0)
        );

        labelPublicIP.DoubleClick += CopyIPFromLabel;

        labelPrivateIP.Text = $"Private IP: {intIP}";
        labelPrivateIP.DoubleClick += CopyIPFromLabel;

        BackgroundWorker worker = new();
        worker.DoWork += async (s, e) =>
        {
            while (true)
            {
                DataGridMethods.GetSuggestedPorts(dataGridSuggestionView);
                await Task.Delay(2500);
            }
        };
        worker.RunWorkerAsync();
    }

    private void CopyIPFromLabel(object sender, EventArgs e)
    {
        var ip = ((Label)sender) == labelPrivateIP ? intIP : extIP;
        ShowNotif($"Copied IP {ip} to clipboard.", 3000);
        Clipboard.SetText(ip);
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
        if (device is null)
            return;
        IPDataObject port = e.Row.DataBoundItem as IPDataObject;
        if (port.Protocol != "DYN")
        {
            await device.DeletePortMapAsync(new Mapping(Enum.Parse<Protocol>(port.Protocol, true), port.PrivatePort, port.PublicPort, int.MaxValue, textBoxDescription.Text));
            return;
        }
        await device.DeletePortMapAsync(new Mapping(Protocol.Tcp, port.PrivatePort, port.PublicPort, int.MaxValue, textBoxDescription.Text));
        await device.DeletePortMapAsync(new Mapping(Protocol.Udp, port.PrivatePort, port.PublicPort, int.MaxValue, textBoxDescription.Text));
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
        if (device is null)
            return;
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
                for (var udp = 0; udp < 2; udp++)
                {
                    if (udp == 0 ? checkBoxTCP.Checked : checkBoxUDP.Checked)
                    {
                        Mapping returnedMap;
                        var protocol = udp == 0 ? Protocol.Tcp : Protocol.Udp;
                        try
                        {
                            await device.GetSpecificMappingAsync(protocol, startPort + i);
                            ShowNotif($"Private {protocol.ToString().ToUpperInvariant()} port: {startPublicPort + i} already mapped!", 3000, true);
                        }
                        catch (MappingException)
                        {
                            returnedMap = await device.CreatePortMapAsync(new Mapping(protocol, startPort + i, startPublicPort + i, int.MaxValue - 1, textBoxDescription.Text != "" ? textBoxDescription.Text : " "));
                            if (returnedMap.PrivatePort != startPort + i || returnedMap.PublicPort != startPublicPort + i)
                            {
                                await device.DeletePortMapAsync(returnedMap);
                                throw new MappingException();
                            }
                        }
                    }
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
        if (device is null)
            return;
        DataGridMethods.GenerateRows(dataGridPortsView, device);
        DataGridMethods.GetSuggestedPorts(dataGridSuggestionView);
    }
}
