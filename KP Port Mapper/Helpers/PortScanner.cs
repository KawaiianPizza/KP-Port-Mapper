namespace KP_Port_Mapper;

internal static partial class PortScanner
{
    private static readonly Regex portRegex = NetStatReg();

    internal static List<LocalPort> GetLocalPorts()
    {
        var netStatOutput = NetStat();
        List<LocalPort> allPorts = portRegex.Matches(netStatOutput)
            .Select(match => new LocalPort
            {
                Protocol = match.Groups["Protocol"].Value,
                Port = match.Groups["Port"].Value,
                Process = match.Groups["Process"].Value
            })
            .ToList();

        Dictionary<string, LocalPort> uniquePorts = [];
        foreach (var item in allPorts)
            if (!uniquePorts.TryGetValue($"{item.Process}_{item.Port}", out LocalPort trimmedItem))
                uniquePorts.Add($"{item.Process}_{item.Port}", item);
            else if (trimmedItem.Protocol != item.Protocol)
                trimmedItem.Protocol = "DYN";

        return [.. uniquePorts.Values];
    }

    internal static string[] GetBlackList()
    {
        var blackList = new List<string>();
        if (File.Exists("blacklist.txt"))
            blackList.AddRange(File.ReadLines("blacklist.txt"));
        return [Path.GetFileName(Environment.ProcessPath), ..blackList];
    }

    internal static Dictionary<string, PortSuggestionDataObject> GetPortsFromProcesses(List<LocalPort> localPorts, string[] blacklistedProcesses)
    {
        var ports = new Dictionary<string, PortSuggestionDataObject>();

        foreach (var item in localPorts)
        {
            var process = Process.GetProcessById(int.Parse(item.Process));
            if (!NativeMethods.GetWindowRect(process.MainWindowHandle, out _))
                continue;
            var executablePath = GetExecutablePathAboveVista(process.Id);
            if (blacklistedProcesses.Contains(executablePath))
                continue;

            var portNum = item.Port;
            var protocol = item.Protocol;
            var ps = new PortSuggestionDataObject(portNum, protocol, executablePath, process.MainWindowTitle);

            ports.Add(portNum, ps);
        }

        return ports;
    }

    private static string NetStat()
    {
        using var process = Process.Start(new ProcessStartInfo("netstat.exe", "-a -n -o")
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        });

        using var stdOutput = process.StandardOutput;
        using var stdError = process.StandardError;

        return stdOutput.ReadToEnd() + stdError.ReadToEnd();
    }

    private static string GetExecutablePathAboveVista(int processId)
    {
        IntPtr hProcess = IntPtr.Zero;

        try
        {
            hProcess = NativeMethods.OpenProcess(0x00001000, false, processId);

            if (hProcess == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            var buffer = new StringBuilder(1024);
            var size = buffer.Capacity;

            if (NativeMethods.QueryFullProcessImageName(hProcess, 0, buffer, out size))
                return Path.GetFileName(buffer.ToString());

            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
        finally
        {
            if (hProcess != IntPtr.Zero)
                NativeMethods.CloseHandle(hProcess);
        }
    }

    [GeneratedRegex(@"(?<Protocol>TCP(?=.+LISTENING)|UDP)    [\[\]*:\.\d]+:(?<Port>\d{2,}).+ (?<Process>\d{2,})\r\n", RegexOptions.Multiline | RegexOptions.Compiled)]
    private static partial Regex NetStatReg();
}
