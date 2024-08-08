using Yona.Core.App;
using Yona.Core.Audio.Models;
using Yona.Core.Common.Serializers;

namespace Yona.Core.Audio;

public class LoopService
{
    private readonly string loopsDir;

    public LoopService(AppService app)
    {
        this.loopsDir = Path.Join(app.BaseDir, "audio", "loops");
        Directory.CreateDirectory(this.loopsDir);
    }

    /// <summary>
    /// Gets saved loop settings for the given file.
    /// </summary>
    /// <param name="file">File to get loop for.</param>
    /// <returns>Saved loop settings or new loop.</returns>
    public Loop GetLoop(string file)
    {
        var loopFile = this.GetLoopFile(file);
        if (loopFile == null)
        {
            return new();
        }

        var loop = JsonFileSerializer.Instance.DeserializeFile<Loop>(loopFile);
        return loop;
    }

    /// <summary>
    /// Saves loop settings for the given file.
    /// </summary>
    /// <param name="file">File to save loop for.</param>
    /// <param name="loop">Loop settings.</param>
    /// <param name="outputLoopFile">Optional output loop file.</param>
    public void SaveLoop(string file, Loop loop, string? outputLoopFile = null)
    {
        JsonFileSerializer.Instance.SerializeFile(this.GetFileLoopPath(file), loop);
        if (outputLoopFile != null)
        {
            JsonFileSerializer.Instance.SerializeFile(outputLoopFile, loop);
        }
    }

    private string? GetLoopFile(string file)
    {
        // Loop settings saved by app.
        var loopFile = this.GetFileLoopPath(file);
        if (File.Exists(loopFile))
        {
            return loopFile;
        }

        // Loop settings relative to file.
        var relLoopFile = $"{file}.json";
        if (File.Exists(relLoopFile))
        {
            return relLoopFile;
        }

        return null;
    }

    private string GetFileLoopPath(string file) => Path.Join(this.loopsDir, $"{Path.GetFileName(file)}.json");
}
