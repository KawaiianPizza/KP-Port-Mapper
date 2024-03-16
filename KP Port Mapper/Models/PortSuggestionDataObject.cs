namespace KP_Port_Mapper;

internal class PortSuggestionDataObject(string port, string protocol, string process, string title)
{
    public string Port { get; set; } = port;

    public string Protocol { get; set; } = protocol;
    public string Process { get; set; } = process;
    public string Title { get; set; } = title;

    public static bool operator ==(PortSuggestionDataObject o1, PortSuggestionDataObject o2) => o1.Port == o2.Port && o1.Process == o2.Process && o1.Protocol == o2.Protocol && o1.Title == o2.Title;
    public static bool operator !=(PortSuggestionDataObject o1, PortSuggestionDataObject o2) => !(o1 == o2);

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
            return true;
        if (obj is null || GetType() != obj.GetType())
            return false;

        var other = (PortSuggestionDataObject)obj;
        return Port == other.Port && Process == other.Process && Protocol == other.Protocol && Title == other.Title;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + (Port?.GetHashCode() ?? 0);
            hash = hash * 23 + (Process?.GetHashCode() ?? 0);
            hash = hash * 23 + (Protocol?.GetHashCode() ?? 0);
            hash = hash * 23 + (Title?.GetHashCode() ?? 0);
            return hash;
        }
    }
}
