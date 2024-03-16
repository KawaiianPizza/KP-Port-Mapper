using KP_Port_Mapper.Helpers;
using Mono.Nat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KP_Port_Mapper;
public partial class FormKPPortMapper : Form
{
    private readonly NotificationHelper labelHelper;
    public FormKPPortMapper()
    {
        foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (nic.OperationalStatus != OperationalStatus.Up)
                continue;
            var ipProps = nic.GetIPProperties().GatewayAddresses;
            foreach (var gateway in ipProps)
            {
                NatUtility.Search(gateway.Address, NatProtocol.Pmp);
                NatUtility.Search(gateway.Address, NatProtocol.Upnp);
                Trace.WriteLine($"Interface: {nic.Name}, Gateway: {gateway.Address}");
            }
        }
        NatUtility.DeviceFound += PortMappingManager.DeviceFound;
        NatUtility.DeviceFound += async (s, e) =>
        {
            Trace.WriteLine(e.Device);
            if (PortMappingManager.device == null)
                return;

            labelDisabled.Invoke(new Action(() => labelDisabled.Visible = false));
            labelNATPMP.Invoke(new Action(() => labelNATPMP.Visible = e.Device.NatProtocol == NatProtocol.Pmp));
            labelPublicIP.Invoke(new Action(() => labelPublicIP.Text = $"IP Address is: {PortMappingManager.extIP}"));

            await DataGridExtensions.GenerateRows(dataGridPortsView, PortMappingManager.device);
        };

        labelHelper = new NotificationHelper(this);

        SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        UpdateStyles();
        InitializeComponent();
    }

    private void FormKPPortMapper_Load(object sender, EventArgs e)
    {
        DataGridExtensions.ConfigureAndAlignColumns(dataGridPortsView,
            ("Protocol", "Type", 42),
            ("PrivatePort", "Private", 70),
            ("PublicPort", "Public", 60),
            ("Description", "Description", 0) // 0 for AutoSizeMode.Fill
        );
        DataGridExtensions.ConfigureAndAlignColumns(dataGridSuggestionView,
            ("Port", "Port", 56),
            ("Protocol", "Type", 42),
            ("Process", "Process", 0),
            ("Title", "Title", 0)
        );

        labelPrivateIP.Text = $"Private IP: {PortMappingManager.intIP}";

        BackgroundWorker worker = new();
        worker.DoWork += async (s, e) =>
        {
            while (true)
            {
                DataGridExtensions.GetSuggestedPorts(dataGridSuggestionView);
                await Task.Delay(2500);
            }
        };
        worker.RunWorkerAsync();
    }

    private void CopyIPFromLabel_DoubleClick(object sender, EventArgs e)
    {
        var ip = ((Label)sender) == labelPrivateIP ? PortMappingManager.intIP : PortMappingManager.extIP;
        labelHelper.ShowNotification($"Copied IP {ip} to clipboard.", 3000);
        Clipboard.SetText(ip);
    }

    private void DataGridPortsView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        DataGridViewRow dgvRow = (sender as DataGridView).Rows[e.RowIndex];
        IPDataObject ip = dgvRow.DataBoundItem as IPDataObject;
        string text = e.ColumnIndex == 1 ? $"{PortMappingManager.intIP}:{ip.PrivatePort}" : $"{PortMappingManager.extIP}:{ip.PublicPort}";
        labelHelper.ShowNotification($"Copied IP {text} to clipboard.", 3000);
        Clipboard.SetText(text);
    }


    private void DataGridSuggestionView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        DataGridViewRow dgvRow = (sender as DataGridView).Rows[e.RowIndex];
        PortSuggestionDataObject ps = dgvRow.DataBoundItem as PortSuggestionDataObject;
        textboxPrivatePortMin.Text = ps.Port;
        textBoxDescription.Text = ps.Process;
        checkBoxTCP.Checked = ps.Protocol == "TCP";
        checkBoxUDP.Checked = ps.Protocol == "UDP";
    }

    private async void DataGridPortsView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
        if (PortMappingManager.device is null)
            return;
        IPDataObject port = e.Row.DataBoundItem as IPDataObject;
        if (port.Protocol != "DYN")
        {
            await PortMappingManager.device.DeletePortMapAsync(new Mapping(Enum.Parse<Protocol>(port.Protocol, true), port.PrivatePort, port.PublicPort, int.MaxValue, textBoxDescription.Text));
            return;
        }
        await PortMappingManager.device.DeletePortMapAsync(new Mapping(Protocol.Tcp, port.PrivatePort, port.PublicPort, int.MaxValue, textBoxDescription.Text));
        await PortMappingManager.device.DeletePortMapAsync(new Mapping(Protocol.Udp, port.PrivatePort, port.PublicPort, int.MaxValue, textBoxDescription.Text));
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
            PortMappingManager.ChangePort(tb, ushort.MaxValue.ToString());
            tb.SelectionStart = s;
        }

        int span = Math.Abs(int.Parse("0" + textboxPrivatePortMin.Text) - int.Parse("0" + textboxPrivatePortMax.Text));

        Dictionary<string, (TextBox[], string)> textBoxMapping = new()
        {
            {"textboxPrivatePortMin", (new TextBox[] { textboxPrivatePortMax, textboxPublicPortMin, textboxPublicPortMax }, tb.Text)},
            {"textboxPrivatePortMax", (new TextBox[] { textboxPublicPortMax }, (int.Parse("0" + textboxPublicPortMin.Text) + span).ToString())},
            {"textboxPublicPortMin", (new TextBox[] { textboxPublicPortMax }, (int.Parse("0" + textboxPublicPortMin.Text) + span).ToString())},
            {"textboxPublicPortMax", (new TextBox[] { textboxPublicPortMin }, (int.Parse("0" + textboxPublicPortMax.Text) - span).ToString())},
        };

        if (textBoxMapping.TryGetValue(tb.Name, out (TextBox[], string) value))
        {
            (TextBox[] textBoxes, string text) = value;
            foreach (TextBox textBox in textBoxes)
                PortMappingManager.ChangePort(textBox, text);
        }
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
        if (PortMappingManager.device is null)
            return;

        int startPort = PortMappingManager.GetValidPort(textboxPrivatePortMin.Text, textboxPrivatePortMax.Text);
        if (startPort == 0)
            return;

        int startPublicPort = PortMappingManager.GetValidPort(textboxPublicPortMin.Text);
        if (startPublicPort == 0)
            return;

        int span = Math.Abs(startPort - PortMappingManager.GetValidPort(textboxPrivatePortMax.Text));

        int realCount = span * (Convert.ToInt32(checkBoxTCP.Checked) + Convert.ToInt32(checkBoxUDP.Checked));
        if (realCount > 10 && MessageBox.Show($"Are you sure you want to open {realCount} ports?") != DialogResult.OK)
            return;
        List<Mapping> mappings = [];
        for (int i = 0; i <= span; i++)
        {
            try
            {
                if (checkBoxTCP.Checked)
                    mappings.Add(await PortMappingManager.MapPortAsync(Protocol.Tcp, startPort + i, startPublicPort + i, textBoxDescription.Text));
                if (checkBoxUDP.Checked)
                    mappings.Add(await PortMappingManager.MapPortAsync(Protocol.Udp, startPort + i, startPublicPort + i, textBoxDescription.Text));
            }
            catch (MappingException ex)
            {
                if (ex.ErrorCode == ErrorCode.ConflictInMappingEntry)
                    labelHelper.ShowNotification($"Private port: {startPort + i} already mapped!", 3000, true);
                else
                    labelHelper.ShowNotification($"Public port: {startPublicPort + i} already mapped!", 3000, true);
            }
        }

        if (PortMappingManager.device.NatProtocol == NatProtocol.Upnp)
        {
            await DataGridExtensions.GenerateRows(dataGridPortsView, PortMappingManager.device);
            mappings = null;
            return;
        }
        DataGridExtensions.AddToGrid(dataGridPortsView, mappings.Select(e => new IPDataObject(e.Protocol.ToString(), e.PrivatePort, e.PublicPort, e.Description)));
        DataGridExtensions.UpdateOpenPorts(dataGridPortsView);
    }
}
