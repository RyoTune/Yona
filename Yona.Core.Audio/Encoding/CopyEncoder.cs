using Yona.Core.Audio.Loops;

namespace Yona.Core.Audio.Encoding;

public class CopyEncoder : IEncoder
{
    public CopyEncoder()
    {
        this.EncodedExt = string.Empty;
    }

    public string Name { get; } = "Copy File";

    public string EncodedExt { get; private set; }

    public string[]? InputTypes { get; } = null;

    public async Task Encode(string inputFile, string outputFile, Loop? loop)
    {
        using var source = File.OpenRead(inputFile);
        using var output = File.Create(outputFile);
        await source.CopyToAsync(output);
    }
}
