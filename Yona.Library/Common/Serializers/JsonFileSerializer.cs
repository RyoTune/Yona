using System.Text.Json;

namespace Yona.Library.Common.Serializers;

internal class JsonFileSerializer : IFileSerializer
{
    private readonly static JsonSerializerOptions defaults = new() { WriteIndented = true };

    public static readonly JsonFileSerializer Instance = new();

    public T DeserializeFile<T>(string filePath)
    {
        var data = File.ReadAllBytes(filePath);
        return JsonSerializer.Deserialize<T>(data) ?? throw new Exception();
    }

    public void SerializeFile<T>(string filePath, T obj)
    {
        var data = JsonSerializer.Serialize(obj, defaults);
        File.WriteAllText(filePath, data);
    }
}
