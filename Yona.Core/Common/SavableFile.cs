using Yona.Core.Extensions;
using Yona.Core.Utils.Serializers;

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

        this.Data = this.GetData();
    }

    public T Data { get; set; }

    /// <summary>
    /// Save current data to file.
    /// </summary>
    public void Save() => serializer.SerializeFile(filePath, this.Data);

    /// <summary>
    /// Reload data from file.
    /// </summary>
    public void Reload()
    {
        this.Data = this.GetData();
    }

    private T GetData()
    {
        if (File.Exists(this.filePath))
        {
            try
            {
                return this.serializer.DeserializeFile<T>(this.filePath);
            }
            
            // Backup and reset if failed to load existing file.
            catch (Exception)
            {
                // TODO: Log error.
                return this.BackupAndReset(filePath);
            }
        }

        var defaultValue = new T();
        this.serializer.SerializeFile(this.filePath, defaultValue);
        return defaultValue;
    }

    private T BackupAndReset(string filePath)
    {
        // Back up original file.
        var backupFile = $"{filePath}_{DateTime.Now.ToPathSafe()}.backup";
        File.Copy(filePath, backupFile, true);

        // Write new default.
        var defaultValue = new T();
        this.serializer.SerializeFile(filePath, defaultValue);
        return defaultValue;
    }
}
