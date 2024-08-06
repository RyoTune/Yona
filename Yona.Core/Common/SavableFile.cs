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

        Value = GetValue();
    }

    public T Value { get; set; }

    public void Save() => serializer.SerializeFile(filePath, Value);

    private T GetValue()
    {
        if (File.Exists(filePath))
        {
            return serializer.DeserializeFile<T>(filePath);
        }

        var defaultValue = new T();
        serializer.SerializeFile(filePath, defaultValue);
        return defaultValue;
    }
}
