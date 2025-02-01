using Microsoft.Extensions.Logging;
using Yona.Core.Audio;
using Yona.Core.Audio.Encoding;
using Yona.Core.Audio.Models;
using Yona.Core.Projects.Models;

namespace Yona.Core.Projects.Builders;

public class StandardProjectBuilder(EncoderRepository encoders, ILogger<StandardProjectBuilder> log) : IProjectBuilder
{
    private readonly EncoderRepository encoders = encoders;
    private readonly ILogger log = log;

    public async Task Build(ProjectBundle project, IProgress<float>? progress)
    {
        var outputDir = project.BuildDir;

        var tracks = project.Data.Tracks;
        foreach (var track in tracks)
        {
            if (string.IsNullOrEmpty(track.OutputPath))
            {
                throw new Exception($"Track output path is missing.\nTrack:{track.Name}");
            }

            var outputFile = Path.Join(outputDir, track.OutputPath);
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            if (string.IsNullOrEmpty(track.InputFile))
            {
                continue;
            }

            if (this.encoders.GetEncoder(track.Encoder) is IEncoder encoder)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outputFile)!);

                try
                {
                    await encoder.Encode(track.InputFile!, outputFile, track.Loop.ToModel());
                }
                catch (Exception ex)
                {
                    this.log.LogError(ex, "Failed to encode file: {file}", track.InputFile!);
                    throw;
                }
            }
            else
            {
                this.log.LogError("Unknown encoder {encoderName} for track {trackName}.", track.Encoder, track.Name);
            }
        }
    }
}
