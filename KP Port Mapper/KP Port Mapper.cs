using Open.Nat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KP_Port_Mapper
{
    public partial class formKPPortMapper : Form
    {
        public formKPPortMapper()
        {
            InitializeComponent();
        }

        readonly NatDiscoverer discoverer = new();
        string extIP;
        readonly string intIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();

        NatDevice device;
        private async void Form1_Load(object sender, EventArgs e)
        {
            device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, new CancellationTokenSource(10000));
            extIP = (await device.GetExternalIPAsync()).ToString();

            dataGridPortsView.UserDeletingRow += DataGridPortsView_UserDeletingRow;
            dataGridPortsView.CellDoubleClick += DataGridPortsView_CellDoubleClick;
            dataGridPortsView.Columns.AddRange(new[] {
                new DataGridViewTextBoxColumn { DataPropertyName = "Protocol", Name = "Type", Width = 42, ReadOnly = true },
                new DataGridViewTextBoxColumn { DataPropertyName = "PrivatePort", Name = "Private", Width = 70, ReadOnly = true },
                new DataGridViewTextBoxColumn { DataPropertyName = "PublicPort", Name = "Public", Width = 60, ReadOnly = true },
                new DataGridViewTextBoxColumn { DataPropertyName = "Description", Name = "Description", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true },
            });
            DataGridMethods.AlignColumns(dataGridPortsView.Columns);

            dataGridSuggestionView.CellDoubleClick += DataGridSuggestionView_CellDoubleClick;
            dataGridSuggestionView.Columns.AddRange(new[] {
                new DataGridViewTextBoxColumn { DataPropertyName = "Port", Name = "Port", Width = 56, ReadOnly = true },
                new DataGridViewTextBoxColumn { DataPropertyName = "Protocol", Name = "Type", Width = 42, ReadOnly = true },
                new DataGridViewTextBoxColumn { DataPropertyName = "Process", Name = "Process", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true },
                new DataGridViewTextBoxColumn { DataPropertyName = "Title", Name = "Title", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true },
            });
            DataGridMethods.AlignColumns(dataGridSuggestionView.Columns);

            this.labelPublicIP.Text = $"IP Address is: {extIP}";
            this.labelPublicIP.DoubleClick += (s, e) => { ShowNotif($"Copied IP {extIP} to clipboard.", 3000); Clipboard.SetText(extIP); };
            this.labelPrivateIP.Text = $"Private IP: {intIP}";
            this.labelPrivateIP.DoubleClick += (s, e) => { ShowNotif($"Copied IP {intIP} to clipboard.", 3000); Clipboard.SetText(intIP); };
            DataGridMethods.GenerateRows(dataGridPortsView, device);
            DataGridMethods.GetSuggestedPorts(dataGridSuggestionView);
        }

        private void DataGridPortsView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgvRow = (sender as DataGridView).Rows[e.RowIndex];
            var ip = dgvRow.DataBoundItem as IPDataObject;
            var text = $"{extIP}:{ip.PublicPort}";
            ShowNotif($"Copied IP {text} to clipboard.", 3000);
            Clipboard.SetText(text);
        }
        private bool notifShown = false;
        private void ShowNotif(string text, int duration)
        {
            if (notifShown)
            {
                Controls[0].Text = text;
                return;
            }
            var width = (int)(Width * 0.75);
            var notif = new Label
            {
                Width = width,
                Location = new Point((Width - width) / 2, 15),
                BackColor = Color.LimeGreen,
                Text = text
            };
            this.Controls.Add(notif);
            this.Controls.SetChildIndex(notif, 0);
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
                this.Invoke(new MethodInvoker(() =>
                {
                    notifShown = false;
                    Controls.Remove(notif);
                    notif.Dispose();
                }));
            });
        }

        private void DataGridSuggestionView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgvRow = (sender as DataGridView).Rows[e.RowIndex];
            var ps = dgvRow.DataBoundItem as PortSuggestionDataObject;
            textboxPrivatePortMin.Text = ps.Port;
            textBoxDescription.Text = ps.Process;
            checkBoxTCP.Checked = ps.Protocol != "UDP";
            checkBoxUDP.Checked = ps.Protocol != "TCP";
        }

        private async void DataGridPortsView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var port = e.Row.DataBoundItem as IPDataObject;
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
                    if ((sender as TextBox).Name == "textboxPublicPortMax")
                        break;
                    return;
            }
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void Textbox_TextChanged(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            switch (tb.Name)
            {
                case "textboxPrivatePortMin":
                    if (int.Parse("0" + tb.Text) > ushort.MaxValue)
                        tb.Text = ushort.MaxValue.ToString();
                    textboxPrivatePortMax.Text = textboxPublicPortMin.Text = textboxPublicPortMax.Text = textboxPrivatePortMin.Text;
                    return;
                case "textboxPrivatePortMax":
                    textboxPublicPortMax.Text = textboxPrivatePortMax.Text;
                    break;
                case "textboxPublicPortMin":
                    break;
                case "textboxPublicPortMax":
                    if (int.Parse("0" + tb.Text) > ushort.MaxValue)
                        tb.Text = ushort.MaxValue.ToString();
                    return;
            }
            if (int.Parse("0" + tb.Text) > ushort.MaxValue)
                tb.Text = ushort.MaxValue.ToString();
            var span = Math.Abs(int.Parse("0" + textboxPrivatePortMin.Text) - int.Parse("0" + textboxPrivatePortMax.Text));
            textboxPublicPortMax.Text = (int.Parse("0" + textboxPublicPortMin.Text) + span).ToString();
        }

        private void DataGridPortsView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (e.Value is not "TCP" and not "UDP" and not "DYN")
                return;
            foreach (DataGridViewCell item in dgv.Rows[e.RowIndex].Cells)
            {
                Color color = e.Value.ToString() switch
                {
                    "TCP" => Color.DarkTurquoise,
                    "UDP" => Color.LimeGreen,
                    "DYN" => Color.MediumSeaGreen
                };
                item.Style.BackColor = color;
                item.Style.SelectionBackColor = color;
            }
        }
        private async void ButtonOpenPort_Click(object sender, EventArgs e)
        {
            int startPort = int.Parse("0" + textboxPrivatePortMin.Text);
            if (startPort == 0)
                return;

            int startPublicPort = int.Parse("0" + textboxPublicPortMin.Text);
            if (startPublicPort == 0)
                return;

            var span = Math.Abs(startPort - int.Parse("0" + textboxPrivatePortMax.Text));

            int realCount = span * (Convert.ToInt32(checkBoxTCP.Checked) + Convert.ToInt32(checkBoxUDP.Checked));
            if (realCount > 10 && MessageBox.Show($"Are you sure you want to open {realCount} ports?") != DialogResult.Yes)
                return;

            for (int i = 0; i <= span; i++)
            {
                if (checkBoxTCP.Checked)
                    await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, startPort + i, startPublicPort + i, textBoxDescription.Text != "" ? textBoxDescription.Text : " "));
                if (checkBoxUDP.Checked)
                    await device.CreatePortMapAsync(new Mapping(Protocol.Udp, startPort + i, startPublicPort + i, textBoxDescription.Text != "" ? textBoxDescription.Text : " "));
            }
            DataGridMethods.GenerateRows(dataGridPortsView, device);
        }

        private void ButtonSuggestionRefresh_Click(object sender, EventArgs e)
        {
            DataGridMethods.GenerateRows(dataGridPortsView, device);
        }
    }
}
