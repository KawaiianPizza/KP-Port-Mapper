namespace KP_Port_Mapper.Models;
internal class ProcessInfoObject(int id, string fileName, string title)
{
    public string ID { get; private set; } = id.ToString();
    public string FileName { get; private set; } = fileName;
    public string Title { get; private set; } = title;
}
