using Open.Nat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KP_Port_Mapper
{
    public static class DataGridMethods
    {
        private static readonly string[] blackList = { "msedge.exe","chrome.exe","firefox.exe", "NVIDIA Share.exe", "Steam.exe" };

        internal static async void GenerateRows(DataGridView dataGridPortsView, NatDevice device)
        {
            var formattedGroupList = new List<IPDataObject>();
            Dictionary<string, IPDataObject> ports = new();
            foreach (var mapping in await device.GetAllMappingsAsync())
            {
                var ip = new IPDataObject(mapping.Protocol.ToString().ToUpper(), mapping.PrivatePort, mapping.PublicPort, mapping.Description);
                var privatePort = mapping.PrivatePort.ToString();
                if (ports.ContainsKey(privatePort))
                {
                    ports[privatePort].Protocol = "DYN";
                    continue;
                }
                formattedGroupList.Add(ip);
                ports.Add(privatePort, ip);
            }
            dataGridPortsView.DataSource = new BindingSource() { DataSource = formattedGroupList };
            dataGridPortsView.Invalidate();
            dataGridPortsView.ClearSelection();
        }
        private static readonly Regex reg = new(@"(?<Protocol>TCP(?=.+LISTENING)|UDP)    [\[\]*:\.\d]+:(?<Port>\d{2,}).+ (?<Process>\d{2,})\r\n", RegexOptions.Compiled | RegexOptions.Multiline);
        internal static void GetSuggestedPorts(DataGridView dataGridSuggestionView)
        {
            var openPorts = reg.Matches(NetStat());
            List<PortSuggestionDataObject> netStat = new();
            Dictionary<string, PortSuggestionDataObject> ports = new();
            foreach (var app in Process.GetProcesses())
            {
                if (!NativeMethods.GetWindowRect(app.MainWindowHandle, out _))
                    continue;
                var fileName = GetExecutablePathAboveVista(app.Id);
                if (Array.IndexOf(blackList, fileName) != -1)
                    continue;
                foreach (var match in openPorts.Where(e => e.Groups[3].Value == app.Id.ToString()))
                {
                    var port = match.Groups["Port"].Value;
                    var ps = new PortSuggestionDataObject(port, match.Groups["Protocol"].Value, fileName, app.MainWindowTitle);
                    if (ports.ContainsKey(port))
                    {
                        ports[port].Protocol = "DYN";
                        continue;
                    }
                    ports.Add(port, ps);
                    netStat.Add(ps);
                }
            }
            dataGridSuggestionView.DataSource = new BindingSource() { DataSource = netStat.OrderBy(e => e.Title) };
            dataGridSuggestionView.Invalidate();
            dataGridSuggestionView.ClearSelection();
        }
        internal static void AlignColumns(DataGridViewColumnCollection collection)
        {
            foreach (var obj in collection)
            {
                if (obj is not DataGridViewTextBoxColumn)
                    return;
                var column = obj as DataGridViewTextBoxColumn;
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }
        private static string NetStat()
        {
            using Process p = Process.Start(new ProcessStartInfo("netstat.exe", "-a -n -o") { UseShellExecute = false, CreateNoWindow = true, RedirectStandardInput = true, RedirectStandardOutput = true, RedirectStandardError = true });
            using StreamReader stdOutput = p.StandardOutput;
            using StreamReader stdError = p.StandardError;
            return stdOutput.ReadToEnd() + stdError.ReadToEnd();
        }
        private static string GetExecutablePathAboveVista(int ProcessId)
        {
            IntPtr hprocess = NativeMethods.OpenProcess(0x00001000, false, ProcessId);
            if (hprocess == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            try
            {
                StringBuilder buffer = new(1024);
                int size = buffer.Capacity;
                if (NativeMethods.QueryFullProcessImageName(hprocess, 0, buffer, out size))
                    return Path.GetFileName(buffer.ToString());
            }
            finally
            {
                NativeMethods.CloseHandle(hprocess);
            }

            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }
}
