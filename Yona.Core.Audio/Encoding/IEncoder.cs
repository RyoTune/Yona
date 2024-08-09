using Yona.Core.Audio.Loops;

namespace Yona.Core.Audio.Encoding;

public interface IEncoder
{
    /// <summary>
    /// Gets default file extension of encoded file.
    /// </summary>
    string EncodedExt { get; }

    /// <summary>
    /// Gets list of valid input formats by extension.
    /// </summary>
    string[] InputTypes { get; }

    /// <summary>
    /// Encode file.
    /// </summary>
    /// <param name="inputFile">File to encode.</param>
    /// <param name="outputFile">Output encoded file.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Encode(string inputFile, string outputFile) => this.Encode(inputFile, outputFile, new());

    /// <summary>
    /// Encode file with loop. If loop is enabled but both loop points are zero,
    /// the entire file will loop.
    /// </summary>
    /// <param name="inputFile">File to encode.</param>
    /// <param name="outputFile">Output encoded file.</param>
    /// <param name="loop">Loop settings.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Encode(string inputFile, string outputFile, Loop loop);
}