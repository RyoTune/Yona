namespace Yona.Core.Common.Serializers;

public interface IFileSerializer
{
    void SerializeFile<T>(string filePath, T obj);

    T DeserializeFile<T>(string filePath);
}
