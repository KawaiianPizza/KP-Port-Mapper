namespace KP_Port_Mapper;
public class IPDataObject(string protocol, int privatePort, int publicPort, string description = "")
{
    public string Protocol { get; set; } = protocol;
    public int PrivatePort { get; set; } = privatePort;
    public int PublicPort { get; set; } = publicPort;
    public string Description { get; set; } = description;

    public static bool operator ==(IPDataObject o1, IPDataObject o2) => o1.PrivatePort == o2.PrivatePort && o1.PublicPort == o2.PublicPort && o1.Protocol == o2.Protocol && o1.Description == o2.Description;
    public static bool operator !=(IPDataObject o1, IPDataObject o2) => !(o1 == o2);

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
            return true;
        if (obj is null || obj.GetType() != GetType())
            return false;

        IPDataObject other = (IPDataObject)obj;
        return this == other;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + PrivatePort.GetHashCode();
            hash = hash * 23 + PublicPort.GetHashCode();
            hash = hash * 23 + Protocol.GetHashCode();
            hash = hash * 23 + (Description?.GetHashCode() ?? 0);
            return hash;
        }
    }
}
