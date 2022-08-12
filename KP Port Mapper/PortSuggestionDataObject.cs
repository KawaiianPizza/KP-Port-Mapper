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
    public static bool operator ==(PortSuggestionDataObject o1, PortSuggestionDataObject o2) => o1.Port == o2.Port && o1.Process == o2.Process && o1.Protocol == o2.Protocol && o1.Title == o2.Title;
    public static bool operator !=(PortSuggestionDataObject o1, PortSuggestionDataObject o2) => !(o1 == o2);
}
