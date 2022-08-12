namespace KP_Port_Mapper;
public class IPDataObject
{
    public string Protocol { get; set; }
    public int PrivatePort { get; set; }
    public int PublicPort { get; set; }
    public string Description { get; set; }
    public IPDataObject(string protocol, int privatePort, int publicPort, string description = "")
    {
        Protocol = protocol;
        PrivatePort = privatePort;
        PublicPort = publicPort;
        Description = description;
    }
    public static bool operator ==(IPDataObject o1, IPDataObject o2) => o1.PrivatePort == o2.PrivatePort && o1.PublicPort == o2.PrivatePort && o1.Protocol == o2.Protocol && o1.Description == o2.Description;
    public static bool operator !=(IPDataObject o1, IPDataObject o2) => !(o1 == o2);
}
