using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Yona.Core.Audio;
using Yona.Core.Audio.Models;
using Yona.Core.Common.Dialog;
using Yona.Core.Projects.Builders;
using Yona.Core.Projects.Models;
using Yona.Core.ViewModels.Dashboard.Projects;

namespace Yona.Core.ViewModels.Convert;

public partial class ConvertViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly ILogger<ConvertViewModel> log;
    private readonly ConvertProjectBuilder builder;
    private readonly LoopService loops;

    private readonly ObservableAsPropertyHelper<IEnumerable<AudioTrack>> _filteredTracks;
    private string _selectedEncoder = "HCA";

    public ConvertViewModel(
        ConvertProjectBuilder builder,
        EncoderRepository encoders,
        LoopService loops,
        string[] files,
        ILogger<ConvertViewModel> log)
    {
        this.log = log;
        this.builder = builder;
        this.loops = loops;
        this.Encoders = encoders.AvailableEncoders;

        this.Project = new ProjectBundle(Path.GetTempFileName());

        // Populate project with tracks from files.
        foreach (var file in files)
        {
            var track = new AudioTrack()
            {
                Name = Path.GetFileName(file),
                InputFile = file,
                Loop = loops.GetLoop(file) ?? new(),
            };

            this.Project.Data.Tracks.Add(track);
        }

        // Only show files that:
        // 1. Are valid input files for the current encoder.
        // 2. Won't output a file that clashes with the input file (HCA input -> HCA output)
        //    UNLESS the output folder and is (unlikely) to clash with the input file.
        _filteredTracks = this.WhenAnyValue(x => x.SelectedEncoder, x => x.Project.Data.OutputDir)
            .Select(x => (encoderName: x.Item1, outputDir: x.Item2))
            .Select(values =>
            {
                var encoder = encoders.GetEncoder(values.encoderName)!;
                if (values.outputDir != null)
                {
                    return this.Project.Data.Tracks
                        .Where(x => encoder.InputTypes?.Contains(Path.GetExtension(x.InputFile), StringComparer.OrdinalIgnoreCase) == true);
                }
                else
                {
                    return this.Project.Data.Tracks
                        .Where(x => encoder.EncodedExt?.Equals(Path.GetExtension(x.InputFile), StringComparison.OrdinalIgnoreCase) == false)
                        .Where(x => encoder.InputTypes?.Contains(Path.GetExtension(x.InputFile), StringComparer.OrdinalIgnoreCase) == true);
                }
            })
            .ToProperty(this, x => x.FilteredTracks);

        this.WhenActivated((CompositeDisposable disp) =>
        {
            this.WhenAnyValue(x => x.SelectedEncoder)
                .Subscribe(x =>
                {
                    this.Project.Data.Tracks[0].Encoder = x;
                });

            _filteredTracks.DisposeWith(disp);
        });
    }

    public IEnumerable<AudioTrack> FilteredTracks => _filteredTracks.Value;

    public ProjectBundle Project { get; }

    public IEnumerable<string> Encoders { get; }

    public FolderSelectInteraction OutputFolder { get; } = new();

    public string SelectedEncoder
    {
        get => _selectedEncoder;
        set => this.RaiseAndSetIfChanged(ref _selectedEncoder, value);
    }

    public ViewModelActivator Activator { get; } = new();

    [RelayCommand]
    private async Task Build()
    {
        try
        {
            foreach (var track in this.Project.Data.Tracks)
            {
                this.loops.SaveLoop(track.InputFile!, track.Loop);
            }

            foreach (var track in this.Project.Data.Tracks)
            {
                track.Enabled = this.FilteredTracks.Contains(track);
            }

            var stopwatch = new Stopwatch();

            stopwatch.Start();
            await this.builder.Build(this.Project, null);
            stopwatch.Stop();

#if RELEASE
            if (stopwatch.ElapsedMilliseconds < ProjectTracksViewModel.MIN_BUILD_TIME_MS)
            {
                await Task.Delay((int)(ProjectTracksViewModel.MIN_BUILD_TIME_MS - stopwatch.ElapsedMilliseconds));
            }
#endif

            this.log.LogInformation("{NumFiles} files converted successfully in {Time}ms!", this.FilteredTracks.Count(), stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            this.log.LogError(ex, "Failed to build convert project.");
        }
    }

    [RelayCommand]
    private async Task SelectOutputFolder()
    {
        var folders = await this.OutputFolder.Handle(new() { Title = "Select Output Folder" });
        if (folders.Length == 0)
        {
            return;
        }

        this.Project.Data.OutputDir = folders[0];
    }
}
