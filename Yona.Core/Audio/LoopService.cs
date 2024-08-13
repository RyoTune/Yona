using Yona.Core.App;
using Yona.Core.Audio.Loops;
using Yona.Core.Audio.Models;
using Yona.Core.Utils.Serializers;

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
    public ObservableLoop? GetLoop(string file)
    {
        var loopFile = this.GetLoopFile(file);
        if (loopFile == null)
        {
            return null;
        }

        var loop = new ObservableLoop(LoopSerializer.DeserializeFile(loopFile));
        return loop;
    }

    /// <summary>
    /// Saves loop settings for the given file.
    /// </summary>
    /// <param name="file">File to save loop for.</param>
    /// <param name="loop">Loop settings.</param>
    /// <param name="outputLoopFile">Optional output loop file.</param>
    public void SaveLoop(string file, ObservableLoop loop, string? outputLoopFile = null)
    {
        LoopSerializer.SerializeFile(this.GetLoopFileDat(file), loop.ToModel());
        if (outputLoopFile != null)
        {
            LoopSerializer.SerializeFile(outputLoopFile, loop.ToModel());
        }
    }

    private string? GetLoopFile(string file)
    {
        // Loop settings saved by app.
        var loopFileDat = this.GetLoopFileDat(file);
        if (File.Exists(loopFileDat))
        {
            return loopFileDat;
        }

        // Convert JSON loops to dat.
        var loopFileJson = this.GetLoopFileJson(file);
        if (File.Exists(loopFileJson))
        {
            var jsonLoop = JsonFileSerializer.Instance.DeserializeFile<Loop>(loopFileJson);
            LoopSerializer.SerializeFile(loopFileDat, jsonLoop);
            return loopFileDat;
        }

        // Loop settings relative to file.
        var relLoopFile = $"{file}.json";
        if (File.Exists(relLoopFile))
        {
            var jsonLoop = JsonFileSerializer.Instance.DeserializeFile<Loop>(loopFileJson);
            LoopSerializer.SerializeFile(loopFileDat, jsonLoop);
            return loopFileDat;
        }

        return null;
    }

    private string GetLoopFileDat(string file) => Path.Join(this.loopsDir, $"{Path.GetFileName(file)}.dat");

    private string GetLoopFileJson(string file) => Path.Join(this.loopsDir, $"{Path.GetFileName(file)}.json");
}
