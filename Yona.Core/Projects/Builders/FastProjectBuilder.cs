using Microsoft.Extensions.Logging;
using Yona.Core.Audio;
using Yona.Core.Audio.Encoding;
using Yona.Core.Audio.Models;
using Yona.Core.Projects.Models;
using Yona.Core.Utils.Serializers;

namespace Yona.Core.Projects.Builders;

public class FastProjectBuilder : IProjectBuilder
{
    private readonly EncoderRepository encoders;
    private readonly ILogger log;

    public FastProjectBuilder(EncoderRepository encoders, ILogger<FastProjectBuilder> log)
    {
        this.encoders = encoders;
        this.log = log;
    }

    public async Task Build(ProjectBundle project, IProgress<float>? progress)
    {
        var prevBuild = GetPrevBuild(project);
        var currentBuild = project.Data.Tracks.Where(x => x.InputFile != null).ToArray();

        // Delete output files of tracks without an output file or no longer exist in current build.
        var removedTracks = prevBuild.ExceptBy(currentBuild.Select(x => x.OutputPath), x => x.OutputPath).ToArray();

        foreach (var track in removedTracks)
        {
            var outputFile = GetOutputFile(track, project);
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }
        }

        // Build any tracks that had loop changed or are new in current build.
        var currBuildDict = currentBuild.ToDictionary(x => x.OutputPath!);
        var prevBuildDict = prevBuild.ToDictionary(x => x.OutputPath!);

        var buildTracks = currentBuild.Where(x =>
            {
                if (prevBuildDict.TryGetValue(x.OutputPath!, out var prevTrack))
                {
                    if (x.InputFile == prevTrack.InputFile
                        && x.Loop.Enabled == prevTrack.Loop.Enabled
                        && x.Loop.StartSample == prevTrack.Loop.StartSample
                        && x.Loop.EndSample == prevTrack.Loop.EndSample
                        && x.Encoder == prevTrack.Encoder)
                    {
                        return false;
                    }
                }

                return true;
            })
            .ToArray();

        foreach (var track in buildTracks)
        {
            if (this.encoders.GetEncoder(track.Encoder) is IEncoder encoder)
            {
                var outputFile = GetOutputFile(track, project);
                Directory.CreateDirectory(Path.GetDirectoryName(outputFile)!);
                await encoder.Encode(track.InputFile!, outputFile, track.Loop.ToModel());
            }
            else
            {
                this.log.LogError("Unknown encoder {encoderName} for track {trackName}.", track.Encoder, track.Name);
            }
        }

        LogBuild(buildTracks, removedTracks);
        SaveCurrBuild(project, currentBuild);
    }

    private void LogBuild(AudioTrack[] buildTracks, AudioTrack[] removedTracks)
    {
        this.log.LogDebug("Tracks Built: {count}", buildTracks.Length);
        foreach (var track in buildTracks)
        {
            this.log.LogDebug("Track: {name}", track.Name);
        }

        this.log.LogDebug("Tracks Removed: {count}", removedTracks.Length);
        foreach (var track in removedTracks)
        {
            this.log.LogDebug("Track: {name}", track.Name);
        }
    }

    private static AudioTrack[] GetPrevBuild(ProjectBundle project)
    {
        var buildFile = Path.ChangeExtension(project.FilePath, ".zm");
        if (!File.Exists(buildFile))
        {
            return [];
        }

        return JsonFileSerializer.Instance.DeserializeFile<AudioTrack[]>(buildFile);
    }

    private static void SaveCurrBuild(ProjectBundle project, AudioTrack[] build)
    {
        var buildFile = Path.ChangeExtension(project.FilePath, ".zm");
        JsonFileSerializer.Instance.SerializeFile(buildFile, build);
    }

    private static string GetOutputFile(AudioTrack track, ProjectBundle project)
        => Path.Join(project.BuildDir, track.OutputPath);
}
