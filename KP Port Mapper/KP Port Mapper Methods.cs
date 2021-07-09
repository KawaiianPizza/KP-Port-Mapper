using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KP_Port_Mapper
{
    public partial class formKPPortMapper : Form
    {
        private async void GenerateRows()
        {
            var formattedGroupList = new List<IPDataObject>();
            foreach (var mapping in await device.GetAllMappingsAsync())
                formattedGroupList.Add(new IPDataObject(mapping.Protocol.ToString().ToUpper(), mapping.PrivatePort, mapping.PublicPort, mapping.Description));
            dataGridPortsViewDataSource.DataSource = formattedGroupList;
            dataGridPortsView.DataSource = dataGridPortsViewDataSource;
            dataGridPortsView.Invalidate();
            dataGridPortsView.ClearSelection();
        }
        private void GetSuggestedPorts()
        {
            List<ProcessInfo> runningApps = new();
            foreach (Process process in Process.GetProcesses())
                if (NativeMethods.GetWindowRect(process.MainWindowHandle, out _))
                {
                    string fileName = GetExecutablePathAboveVista(process.Id);
                    if (Array.IndexOf(blackList, fileName) == -1)
                        runningApps.Add(new ProcessInfo(process.Id, fileName, process.MainWindowTitle));
                }

            using Process p = Process.Start(new ProcessStartInfo("netstat.exe", "-a -n -o") { UseShellExecute = false, CreateNoWindow = true, RedirectStandardInput = true, RedirectStandardOutput = true, RedirectStandardError = true });
            using StreamReader stdOutput = p.StandardOutput;
            using StreamReader stdError = p.StandardError;
            string content = stdOutput.ReadToEnd() + stdError.ReadToEnd();

            var openPorts = reg.Matches(content);
            List<PortSuggestionDataObject> ds = new();

            foreach (var item in runningApps)
                foreach (var match in openPorts.Where(e => e.Groups[3].Value == item.ID))
                {
                    PortSuggestionDataObject portSuggestion = new(match.Groups[2].Value, match.Groups[1].Value, item.FileName, item.Title);
                    if (ds.Find(e => e.Port == portSuggestion.Port && e.Protocol == portSuggestion.Protocol && e.Title == portSuggestion.Title) != null)
                        continue;

                    ds.Add(portSuggestion);
                }

            dataGridSuggestionViewDataSource.DataSource = ds;
            dataGridSuggestionView.DataSource = dataGridSuggestionViewDataSource;
            dataGridSuggestionView.Invalidate();
            dataGridSuggestionView.ClearSelection();
        }
        private static void AlignColumns(DataGridViewColumnCollection collection)
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
