namespace KP_Port_Mapper
{
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
    }
}
