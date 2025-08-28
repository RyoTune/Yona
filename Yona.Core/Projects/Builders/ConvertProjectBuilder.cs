using Microsoft.Extensions.Logging;
using Yona.Core.Audio;
using Yona.Core.Audio.Encoding;
using Yona.Core.Audio.Models;
using Yona.Core.Projects.Models;
using Yona.Core.Utils.Serializers;

namespace Yona.Core.Projects.Builders;

public class ConvertProjectBuilder(EncoderRepository encoders, ILogger<ConvertProjectBuilder> log) : IProjectBuilder
{
    /// <summary>
    /// Builds files either next to their input file or the project's output folder.
    /// All tracks are built using the encoder of the first track.
    /// </summary>
    /// <param name="project">Project to build.</param>
    /// <param name="progress">Progress.</param>
    public async Task Build(ProjectBundle project, IProgress<float>? progress)
    {
        var prevBuild = GetPrevBuild(project);
        var currentBuild = project.Data.Tracks.Where(x => x.InputFile != null).ToArray();
        var encoder = encoders.GetEncoder(project.Data.Tracks.First().Encoder)
            ?? throw new Exception($"Failed to get encoder from first track.\nEncoder: {project.Data.Tracks.First().Encoder}");

        // Delete output files of tracks without an output file or no longer exist in current build.
        var removedTracks = prevBuild.ExceptBy(currentBuild.Select(x => x.InputFile), x => x.InputFile).ToArray();

        foreach (var track in removedTracks)
        {
            var outputFile = GetOutputFile(track, project, encoder);
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }
        }

        // Build any tracks that had loop changed or are new in current build.
        var currBuildDict = currentBuild.ToDictionary(x => x.InputFile!);
        var prevBuildDict = prevBuild.ToDictionary(x => x.InputFile!);

        var buildTracks = currentBuild.Where(x =>
        {
            if (x.Enabled == false)
            {
                return false;
            }

            if (prevBuildDict.TryGetValue(x.InputFile!, out var prevTrack))
            {
                if (x.InputFile == prevTrack.InputFile
                    && x.Loop.Enabled == prevTrack.Loop.Enabled
                    && x.Loop.StartSample == prevTrack.Loop.StartSample
                    && x.Loop.EndSample == prevTrack.Loop.EndSample
                    && x.Encoder == prevTrack.Encoder
                    && File.Exists(GetOutputFile(prevTrack, project, encoder)))
                {
                    return false;
                }
            }

            return true;
        }).ToArray();

        foreach (var track in buildTracks)
        {
            var outputFile = GetOutputFile(track, project, encoder);

            try
            {
                await encoder.Encode(track.InputFile!, outputFile, track.Loop.ToModel());
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to encode file: {file}", track.InputFile!);
                throw;
            }
        }

        LogBuild(buildTracks, removedTracks);
        SaveCurrBuild(project, currentBuild);
    }

    private void LogBuild(AudioTrack[] buildTracks, AudioTrack[] removedTracks)
    {
        log.LogDebug("Tracks Built: {count}", buildTracks.Length);
        foreach (var track in buildTracks)
        {
            log.LogDebug("Track: {name}", track.Name);
        }

        log.LogDebug("Tracks Removed: {count}", removedTracks.Length);
        foreach (var track in removedTracks)
        {
            log.LogDebug("Track: {name}", track.Name);
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

    private static string GetOutputFile(AudioTrack track, ProjectBundle project, IEncoder encoder)
    {
        if (project.Data.OutputDir != null)
        {
            return Path.Join(project.Data.OutputDir, Path.ChangeExtension(Path.GetFileName(track.InputFile), encoder.EncodedExt));
        }

        return Path.ChangeExtension(track.InputFile, encoder.EncodedExt) ?? throw new();
    }
}
