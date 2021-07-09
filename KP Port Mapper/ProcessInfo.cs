namespace KP_Port_Mapper
{
    public partial class formKPPortMapper
    {
        private class ProcessInfo
        {
            public string ID { get; private set; }
            public string FileName { get; private set; }
            public string Title { get; private set; }
            public ProcessInfo(int id, string fileName, string title)
            {
                ID = id.ToString();
                FileName = fileName;
                Title = title;
            }
        }
    }
}
