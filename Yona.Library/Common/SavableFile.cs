using Yona.Library.Common.Serializers;

namespace Yona.Library.Common;

public class SavableFile<T>
    where T : new()
{
    private readonly string filePath;
    private readonly IFileSerializer serializer;

    public SavableFile(string filePath, IFileSerializer serializer)
    {
        this.filePath = filePath;
        this.serializer = serializer;

        this.Value = this.GetValue();
    }

    public T Value { get; set; }

    public void Save() => this.serializer.SerializeFile(this.filePath, this.Value);

    private T GetValue()
    {
        if (File.Exists(this.filePath))
        {
            return this.serializer.DeserializeFile<T>(this.filePath);
        }

        var defaultValue = new T();
        this.serializer.SerializeFile(this.filePath, defaultValue);
        return defaultValue;
    }
}
