namespace KP_Port_Mapper;

internal class PortSuggestionDataObject
{
    public string Port { get; set; }

    public string Protocol { get; set; }
    public string Process { get; set; }
    public string Title { get; set; }

    public PortSuggestionDataObject(string port, string protocol, string process, string title)
    {
        Port = port;
        Protocol = protocol;
        Process = process;
        Title = title;
    }
}
