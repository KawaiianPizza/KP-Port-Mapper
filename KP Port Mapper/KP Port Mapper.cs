using Open.Nat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
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
        private BindingSource dataGridPortsViewDataSource = new();

        NatDevice device;
        private async void Form1_Load(object sender, EventArgs e)
        {
            dataGridPortsView.UserDeletingRow += DataGridPortsView_UserDeletingRow;
            dataGridPortsView.Columns.AddRange(new[] {
                new DataGridViewTextBoxColumn { DataPropertyName = "Protocol", Name = "Protocol", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight=20, ReadOnly = true },
                new DataGridViewTextBoxColumn { DataPropertyName = "PrivatePort", Name = "Private Port", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight=15, ReadOnly = true },
                new DataGridViewTextBoxColumn { DataPropertyName = "PublicPort", Name = "Public Port", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight=15, ReadOnly = true },
                new DataGridViewTextBoxColumn { DataPropertyName = "Description", Name = "Description", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight=50, ReadOnly = true },
            });
            foreach (var obj in dataGridPortsView.Columns)
            {
                if (obj is not DataGridViewTextBoxColumn)
                    return;
                var column = obj as DataGridViewTextBoxColumn;
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, new CancellationTokenSource(10000));
            var p = await device.GetAllMappingsAsync();
            GenerateRows();
            var ip = await device.GetExternalIPAsync();

            this.labelPublicIP.Text = $"IP Address is: {ip}";
        }

        private async void DataGridPortsView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var port = e.Row.DataBoundItem as IPDataObject;
            await device.DeletePortMapAsync(new Mapping(Enum.Parse<Protocol>(port.Protocol, true), port.PrivatePort, port.PublicPort, textBoxDescription.Text));
        }

        private async void GenerateRows()
        {
            var formattedGroupList = new List<IPDataObject>();
            foreach (var mapping in await device.GetAllMappingsAsync())
                formattedGroupList.Add(new IPDataObject(mapping.Protocol.ToString().ToUpper(), mapping.PrivatePort, mapping.PublicPort, mapping.Description));
            dataGridPortsViewDataSource.DataSource = formattedGroupList;
            dataGridPortsView.DataSource = dataGridPortsViewDataSource;
            dataGridPortsView.Invalidate();
        }
        public class IPDataObject
        {
            public string Protocol { get; set; }
            public int PrivatePort { get; set; }
            public int PublicPort { get; set; }
            public string Description { get; set; }
            public IPDataObject(string protocol, int privatePort, int publicPort, string description = "")
            {
                Protocol = protocol;
                PrivatePort = privatePort;
                PublicPort = publicPort;
                Description = description;
            }
        }

        private void Textbox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Back:
                case Keys.End:
                case Keys.Home:
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
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
            if (e.Value is not "TCP" and not "UDP")
                return;
            foreach (DataGridViewCell item in dataGridPortsView.Rows[e.RowIndex].Cells)
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
    }
}
