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
using System.Windows.Forms;

namespace KP_Port_Mapper
{
    public partial class formKPPortMapper : Form
    {
        private static readonly string[] blackList = { "msedge.exe", "NVIDIA Share.exe" };
        private static readonly Regex reg = new(@"([TCPUD]{3})    [\[\]*:\.\d]+:(\d{2,}).+?(\d+)\r\n", RegexOptions.Compiled | RegexOptions.Multiline);

        public formKPPortMapper()
        {
            InitializeComponent();
        }

        readonly NatDiscoverer discoverer = new();
        private readonly BindingSource dataGridPortsViewDataSource = new();
        private readonly BindingSource dataGridSuggestionViewDataSource = new();

        NatDevice device;
        private async void Form1_Load(object sender, EventArgs e)
        {
            device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, new CancellationTokenSource(10000));

            dataGridPortsView.UserDeletingRow += DataGridPortsView_UserDeletingRow;
            dataGridPortsView.Columns.AddRange(new[] {
                new DataGridViewTextBoxColumn { DataPropertyName = "Protocol", Name = "Type", Width = 42, ReadOnly = true },
                new DataGridViewTextBoxColumn { DataPropertyName = "PrivatePort", Name = "Private", Width = 70, ReadOnly = true },
                new DataGridViewTextBoxColumn { DataPropertyName = "PublicPort", Name = "Public", Width = 60, ReadOnly = true },
                new DataGridViewTextBoxColumn { DataPropertyName = "Description", Name = "Description", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true },
            });
            AlignColumns(dataGridPortsView.Columns);

            dataGridSuggestionView.CellDoubleClick += DataGridSuggestionView_CellDoubleClick;
            dataGridSuggestionView.Columns.AddRange(new[] {
                new DataGridViewTextBoxColumn { DataPropertyName = "Port", Name = "Port", Width = 56, ReadOnly = true },
                new DataGridViewTextBoxColumn { DataPropertyName = "Protocol", Name = "Type", Width = 42, ReadOnly = true },
                new DataGridViewTextBoxColumn { DataPropertyName = "Process", Name = "Process", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true },
                new DataGridViewTextBoxColumn { DataPropertyName = "Title", Name = "Title", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true },
            });
            AlignColumns(dataGridSuggestionView.Columns);

            this.labelPublicIP.Text = $"IP Address is: {await device.GetExternalIPAsync()}";
            this.labelPrivateIP.Text = $"Private IP: {Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)}";
            GenerateRows();
            GetSuggestedPorts();
        }

        private void DataGridSuggestionView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgvRow = (sender as DataGridView).Rows[e.RowIndex];
            textboxPrivatePortMin.Text = dgvRow.Cells[0].Value.ToString();
            textBoxDescription.Text = dgvRow.Cells[2].Value.ToString();
            bool isTCP = dgvRow.Cells[1].Value.ToString() == "TCP";
            checkBoxTCP.Checked = isTCP;
            checkBoxUDP.Checked = !isTCP;
        }

        private async void DataGridPortsView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var port = e.Row.DataBoundItem as IPDataObject;
            await device.DeletePortMapAsync(new Mapping(Enum.Parse<Protocol>(port.Protocol, true), port.PrivatePort, port.PublicPort, textBoxDescription.Text));
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
                case var _ when e.KeyCode is Keys.C or Keys.V or Keys.X && e.Control:
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
            if (e.Value is not "TCP" and not "UDP")
                return;
            foreach (DataGridViewCell item in dgv.Rows[e.RowIndex].Cells)
            {
                var color = e.Value is "UDP" ? Color.LimeGreen : Color.Turquoise;
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
            GenerateRows();
        }

        private void ButtonSuggestionRefresh_Click(object sender, EventArgs e)
        {
            GetSuggestedPorts();
        }
    }
}
