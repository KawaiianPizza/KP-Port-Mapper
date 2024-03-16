using Mono.Nat;

namespace KP_Port_Mapper.Helpers;
internal static class DataGridExtensions
{
    private static readonly BindingSource openPorts = new() { DataSource = new List<IPDataObject>() };
    private static Task refresh = null;
    private static readonly BindingSource suggestedPorts = new() { DataSource = new List<PortSuggestionDataObject>() };

    internal static async Task GenerateRows(DataGridView dataGridPortsView, INatDevice device)
    {
        if (refresh != null && !refresh.IsCompleted)
            return;
        if (device.NatProtocol == NatProtocol.Pmp)
            return;
        refresh = RefreshPortsAsync(dataGridPortsView, device);
        await refresh;
    }

    private static async Task RefreshPortsAsync(DataGridView dataGridPortsView, INatDevice device)
    {
        Dictionary<string, IPDataObject> ports = [];
        IEnumerable<Mapping> mappings = await device.GetAllMappingsAsync();

        foreach (Mapping mapping in mappings)
        {
            IPDataObject ip = new(mapping.Protocol.ToString().ToUpper(), mapping.PrivatePort, mapping.PublicPort, mapping.Description);
            string privatePort = mapping.PrivatePort.ToString();
            if (ports.TryGetValue(privatePort, out IPDataObject value))
            {
                value.Protocol = "DYN";
                continue;
            }
            ports.Add(privatePort, ip);
        }

        var existingPorts = ((List<IPDataObject>)openPorts.DataSource).ToHashSet();
        var newPorts = ports.Values.ToHashSet();

        var portsToRemove = SafeExcept(existingPorts, newPorts);
        foreach (var item in portsToRemove)
            dataGridPortsView.Invoke(() => openPorts.Remove(item));

        var portsToAdd = SafeExcept(newPorts, existingPorts);
        AddToGrid(dataGridPortsView, portsToAdd);

        UpdateOpenPorts(dataGridPortsView);
    }

    internal static void UpdateOpenPorts(DataGridView dataGridPortsView)
    {
        if (dataGridPortsView.DataSource == null)
            dataGridPortsView.Invoke(() => dataGridPortsView.DataSource = openPorts);
    }

    internal static void AddToGrid(DataGridView dataGridPortsView, IEnumerable<IPDataObject> portsToAdd)
    {
        foreach (var item in portsToAdd)
            dataGridPortsView.Invoke(() => openPorts.Add(item));
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

    internal static void ConfigureAndAlignColumns(DataGridView dataGridView, params (string DataPropertyName, string Name, int Width)[] columns)
    {
        dataGridView.Columns.AddRange(columns
            .Select(column => new DataGridViewTextBoxColumn
            {
                DataPropertyName = column.DataPropertyName,
                Name = column.Name,
                Width = column.Width,
                AutoSizeMode = column.Width == 0 ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.NotSet,
                ReadOnly = true
            })
            .ToArray());

        AlignColumns(dataGridView.Columns);
    }
    internal static void GetSuggestedPorts(DataGridView dataGridSuggestionView)
    {
        var localPorts = PortScanner.GetLocalPorts();
        var blackList = PortScanner.GetBlackList();
        var ports = PortScanner.GetPortsFromProcesses(localPorts, blackList);

        RemoveObsoletePorts(dataGridSuggestionView, ports);
        AddNewPorts(dataGridSuggestionView, ports);

        UpdateDataSource(dataGridSuggestionView);
    }

    private static void RemoveObsoletePorts(DataGridView dataGridSuggestionView, Dictionary<string, PortSuggestionDataObject> ports)
    {
        var portsToRemove = SafeExcept(suggestedPorts.DataSource as List<PortSuggestionDataObject>, [.. ports.Values]);
        foreach (var itemToRemove in portsToRemove)
            dataGridSuggestionView.Invoke(() => suggestedPorts.Remove(itemToRemove));
    }


    private static void AddNewPorts(DataGridView dataGridSuggestionView, Dictionary<string, PortSuggestionDataObject> ports)
    {
        var portsToAdd = SafeExcept([.. ports.Values], suggestedPorts.DataSource as List<PortSuggestionDataObject>);
        foreach (var itemToAdd in portsToAdd)
            dataGridSuggestionView.Invoke(() => suggestedPorts.Add(itemToAdd));
    }
    private static IEnumerable<T> SafeExcept<T>(IEnumerable<T> list, IEnumerable<T> compare)
    {
        var itemsToRemove = new List<T>(list);
        return itemsToRemove.Except(compare);
    }

    private static void UpdateDataSource(DataGridView dataGridSuggestionView)
    {
        if (dataGridSuggestionView.DataSource == null)
            dataGridSuggestionView.Invoke(() => dataGridSuggestionView.DataSource = suggestedPorts);
    }
}
