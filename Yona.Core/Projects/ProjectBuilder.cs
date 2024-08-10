using Microsoft.Extensions.Logging;
using Yona.Core.Audio;
using Yona.Core.Audio.Encoding;
using Yona.Core.Projects.Models;

namespace Yona.Core.Projects;

public class ProjectBuilder
{
    private readonly EncoderRepository encoders;
    private readonly ILogger log;

    public ProjectBuilder(EncoderRepository encoders, ILogger log)
    {
        this.encoders = encoders;
        this.log = log;
    }

    public async Task Build(ProjectBundle project)
    {
        var outputDir = project.Data.OutputDir ?? project.BuildDir;

        var tracks = project.Data.Tracks;
        foreach (var track in tracks)
        {
            if (string.IsNullOrEmpty(track.OutputPath))
            {
                throw new Exception($"Track output path is missing.\nTrack:{track.Name}");
            }

            if (string.IsNullOrEmpty(track.InputFile))
            {
                continue;
            }

            var outputFile = Path.Join(outputDir, track.OutputPath);
            Directory.CreateDirectory(Path.GetDirectoryName(outputFile)!);

            if (this.encoders.GetEncoder(track.Encoder) is IEncoder encoder)
            {
                await encoder.Encode(track.InputFile, outputFile, track.Loop.Model);
            }
            else
            {
                this.log.LogError("Unknown encoder {encoderName} for track {trackName}.", track.Encoder, track.Name);
            }
        }
    }
}
