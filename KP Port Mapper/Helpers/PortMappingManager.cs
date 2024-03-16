using Mono.Nat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KP_Port_Mapper.Helpers;
internal class PortMappingManager
{
    internal static string extIP;
    internal static readonly string intIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
    internal static INatDevice device;
    internal static void ChangePort(TextBox textBox, string text)
    {
        if (textBox.Text == text)
            return;
        textBox.PasswordChar = ' ';
        textBox.Text = text;
    }
    internal static void DeviceFound(object sender, DeviceEventArgs args)
    {
        device ??= args.Device;
        if (args.Device.NatProtocol == NatProtocol.Upnp)
            device = args.Device;

        extIP = device.GetExternalIP().ToString();
    }
    internal static async Task<Mapping> MapPortAsync(Protocol protocol, int privatePort, int publicPort, string description)
    {
        var returnedMap = await device.CreatePortMapAsync(new Mapping(protocol, privatePort, publicPort, int.MaxValue - 1, description));

        if (returnedMap.PrivatePort != privatePort || returnedMap.PublicPort != publicPort)
        {
            await device.DeletePortMapAsync(returnedMap);
            MessageBox.Show($"Mappint to port failed! Expected: priv{privatePort}pub{publicPort} Got: priv{returnedMap.PrivatePort}pub{returnedMap.PublicPort}");
        }
        return returnedMap;
    }
    internal static int GetValidPort(params string[] portTexts) => portTexts.Select(pt => int.Parse("0" + pt)).FirstOrDefault(port => port != 0);
}
