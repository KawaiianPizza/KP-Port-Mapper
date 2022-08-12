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

namespace KP_Port_Mapper;
public static class DataGridMethods
{
    private static readonly string[] blackList = { "msedge.exe", "chrome.exe", "firefox.exe", "NVIDIA Share.exe", "steam.exe" };

    private static readonly BindingSource openPorts = new() { DataSource = new List<IPDataObject>() };
    private static Task refresh = null;
    internal static void GenerateRows(DataGridView dataGridPortsView, NatDevice device)
    {
        if (refresh != null && !refresh.IsCompleted)
            return;
        refresh = Task.Run(async () =>
        {
            Dictionary<string, IPDataObject> ports = new();
            IEnumerable<Mapping> mappings = await device.GetAllMappingsAsync();
            foreach (Mapping mapping in mappings)
            {
                IPDataObject ip = new(mapping.Protocol.ToString().ToUpper(), mapping.PrivatePort, mapping.PublicPort, mapping.Description);
                string privatePort = mapping.PrivatePort.ToString();
                if (ports.ContainsKey(privatePort))
                {
                    ports[privatePort].Protocol = "DYN";
                    continue;
                }
                ports.Add(privatePort, ip);
            }
            foreach (IPDataObject item in new List<IPDataObject>((List<IPDataObject>)openPorts.DataSource))
                if (!ports.Values.Any(e => e == item))
                    dataGridPortsView.Invoke(() => openPorts.Remove(item));
            foreach (IPDataObject item in ports.Values)
                if (!((List<IPDataObject>)openPorts.DataSource).Any(e => e == item))
                    dataGridPortsView.Invoke(() => openPorts.Add(item));
            if (dataGridPortsView.DataSource == null)
                dataGridPortsView.Invoke(() => dataGridPortsView.DataSource = openPorts);
            ports.Clear();
        });
    }

    private static readonly BindingSource suggestedPorts = new() { DataSource = new List<PortSuggestionDataObject>() };
    private static readonly Regex reg = new(@"(?<Protocol>TCP(?=.+LISTENING)|UDP)    [\[\]*:\.\d]+:(?<Port>\d{2,}).+ (?<Process>\d{2,})\r\n", RegexOptions.Compiled | RegexOptions.Multiline);
    internal static void GetSuggestedPorts(DataGridView dataGridSuggestionView)
    {
        MatchCollection localPorts = reg.Matches(NetStat());
        Dictionary<string, PortSuggestionDataObject> ports = new();
        foreach (Process app in Process.GetProcesses())
        {
            if (!NativeMethods.GetWindowRect(app.MainWindowHandle, out _))
                continue;
            string fileName = GetExecutablePathAboveVista(app.Id);
            if (Array.IndexOf(blackList, fileName) != -1)
                continue;
            foreach (Match match in localPorts.Where(e => e.Groups[3].Value == app.Id.ToString()))
            {
                string port = match.Groups["Port"].Value;
                PortSuggestionDataObject ps = new(port, match.Groups["Protocol"].Value, fileName, app.MainWindowTitle);
                if (ports.ContainsKey(port))
                {
                    ports[port].Protocol = "DYN";
                    continue;
                }
                ports.Add(port, ps);
            }
        }
        foreach (PortSuggestionDataObject item in new List<PortSuggestionDataObject>((List<PortSuggestionDataObject>)suggestedPorts.DataSource))
            if (!ports.Values.Any(e =>e == item))
                dataGridSuggestionView.Invoke(() => suggestedPorts.Remove(item));
        foreach (PortSuggestionDataObject item in ports.Values)
            if (!((List<PortSuggestionDataObject>)suggestedPorts.DataSource).Any(e =>e == item))
                dataGridSuggestionView.Invoke(() => suggestedPorts.Add(item));

        if (dataGridSuggestionView.DataSource == null)
            dataGridSuggestionView.Invoke(() => dataGridSuggestionView.DataSource = suggestedPorts);
        ports.Clear();
        localPorts = null;
    }
    internal static void AlignColumns(DataGridViewColumnCollection collection)
    {
        foreach (object obj in collection)
        {
            if (obj is not DataGridViewTextBoxColumn)
                return;
            DataGridViewTextBoxColumn column = obj as DataGridViewTextBoxColumn;
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
