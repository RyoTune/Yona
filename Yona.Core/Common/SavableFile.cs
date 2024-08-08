using Yona.Core.Common.Serializers;

namespace Yona.Core.Common;

public class SavableFile<T>
    where T : new()
{
    private readonly string filePath;
    private readonly IFileSerializer serializer;

    public SavableFile(string filePath, IFileSerializer serializer)
    {
        this.filePath = filePath;
        this.serializer = serializer;

        this.Data = GetValue();
    }

    public T Data { get; set; }

    public void Save() => serializer.SerializeFile(filePath, this.Data);

    private T GetValue()
    {
        if (File.Exists(filePath))
        {
            return serializer.DeserializeFile<T>(filePath);
        }

        var defaultValue = new T();
        serializer.SerializeFile(this.filePath, defaultValue);
        return defaultValue;
    }
}
