using Microsoft.Extensions.Logging;
using Yona.Core.Audio.Loops;
using Yona.Core.Utils;

namespace Yona.Core.Audio.Encoding;

/// <summary>
/// Encoder wrapper that caches encoded files.
/// </summary>
public class CachedEncoder : IEncoder
{
    private readonly ILogger? log;
    private readonly IEncoder encoder;
    private readonly string cachedFolder;

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedEncoder"/> class.
    /// </summary>
    /// <param name="encoder"></param>
    /// <param name="cachedFolder"></param>
    /// <param name="log"></param>
    public CachedEncoder(IEncoder encoder, string cachedFolder, ILogger? log = null)
    {
        this.log = log;
        this.encoder = encoder;
        this.cachedFolder = cachedFolder;
    }

    public string Name => this.encoder.Name;

    public string EncodedExt => this.encoder.EncodedExt;

    public string[]? InputTypes => this.encoder.InputTypes;

    public async Task Encode(string inputFile, string outputFile, Loop? loop)
    {
        var inputChecksum = Checksum.Compute(inputFile);

        var cachedFile = Path.Join(this.cachedFolder, $"{inputChecksum}{this.encoder.EncodedExt}");
        var cachedLoop = loop ?? new();
        var cachedLoopFile = Path.ChangeExtension(cachedFile, ".dat");

        if (!File.Exists(cachedFile) || LoopChanged(cachedLoopFile, cachedLoop))
        {
            this.log?.LogDebug("Cached file missing or loop mismatch.");
            await this.encoder.Encode(inputFile, cachedFile, cachedLoop);
            LoopSerializer.SerializeFile(cachedLoopFile, cachedLoop);
        }

        // Copy cached file to output.
        await Task.Run(() =>
        {
            File.Copy(cachedFile, outputFile, true);
        });
    }

    private static bool LoopChanged(string loopFile, Loop loop)
    {
        if (!File.Exists(loopFile))
        {
            return true;
        }

        var cachedLoop = LoopSerializer.DeserializeFile(loopFile);

        return cachedLoop.StartSample != loop.StartSample
            || cachedLoop.EndSample != loop.EndSample
            || cachedLoop.Enabled != loop.Enabled;
    }
}
