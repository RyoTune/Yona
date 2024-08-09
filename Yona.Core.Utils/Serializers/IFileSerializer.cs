namespace Yona.Core.Utils.Serializers;

public interface IFileSerializer
{
    void SerializeFile<T>(string filePath, T obj);

    T DeserializeFile<T>(string filePath);
}
