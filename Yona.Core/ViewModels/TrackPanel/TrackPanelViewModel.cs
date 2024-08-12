using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Windows.Input;
using Yona.Core.Audio.Models;
using System.Reactive.Linq;
using DynamicData.Binding;
using Yona.Core.Audio;
using Yona.Core.Common.Dialog;
using Yona.Core.Settings;
using Yona.Core.Extensions;
using Yona.Core.Projects.Models;
using Yona.Core.ViewModels.CreateTrack;
using System.Reactive;

namespace Yona.Core.ViewModels.TrackPanel;

public partial class TrackPanelViewModel : ViewModelBase, IActivatableViewModel
{
    private const string NoInputFile = "None";

    private readonly ProjectBundle project;
    private readonly LoopService _loops;
    private readonly EncoderRepository _encoders;
    private readonly ObservableAsPropertyHelper<bool> _isLoopInputEnabled;
    private readonly ObservableAsPropertyHelper<bool> _isDevMode;
    private string _selectedInputFile;

    public TrackPanelViewModel(
        AudioTrack track,
        ProjectBundle project,
        LoopService loops,
        EncoderRepository encoders,
        SettingsService settings,
        ICommand closeCommand)
    {
        this.project = project;
        this.Track = track;
        this._loops = loops;
        this._encoders = encoders;

        this.Encoders = encoders.AvailableEncoders;
        this.CloseCommand = closeCommand;

        // Set current input selection.
        if (track.InputFile != null)
        {
            this.InputFileOptions.Add(track.InputFile);
            this._selectedInputFile = track.InputFile;
        }
        else
        {
            this._selectedInputFile = NoInputFile;
        }

        this._isLoopInputEnabled = this.WhenAnyValue(x => x.Track.InputFile, x => x.Track.Loop.Enabled, (file, loopEnabled) => file != null && loopEnabled)
            .ToProperty(this, x => x.IsLoopInputEnabled);

        this._isDevMode = settings.WhenAnyValue(x => x.Current.IsDevMode).ToProperty(this, x => x.IsDevMode);

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            var trackObs = track.WhenAnyPropertyChanged();
            var loopObs = track.Loop.WhenAnyPropertyChanged();

            // Save loop.
            loopObs.Throttle(TimeSpan.FromMilliseconds(SavableFileExtensions.SAVE_BUFFER_MS))
            .Subscribe(_ =>
            {
                if (track.InputFile != null)
                {
                    loops.SaveLoop(track.InputFile, track.Loop);
                }
            })
            .DisposeWith(disposables);

            // Save project on changes made.
            Observable.Merge<object?>(trackObs, loopObs)
            .Throttle(TimeSpan.FromMilliseconds(SavableFileExtensions.SAVE_BUFFER_MS))
            .Subscribe(values => project.Save())
            .DisposeWith(disposables);

            // Set track properties on selection made.
            this.WhenAnyValue(x => x.SelectedInputFile)
            .Subscribe(file =>
            {
                if (file == null || file == NoInputFile)
                {
                    this.Track.InputFile = null;
                    this.Track.Loop.Enabled = false;
                    this.Track.Loop.StartSample = 0;
                    this.Track.Loop.EndSample = 0;
                }
                else
                {
                    this.Track.InputFile = file;
                    this.Track.Loop.Enabled = project.Data.DefaultLoopState;

                    var existingLoop = loops.GetLoop(file);
                    if (existingLoop != null)
                    {
                        if (existingLoop.StartSample != 0 && existingLoop.EndSample != 0)
                        {
                            this.Track.Loop.Enabled = true;
                        }

                        this.Track.Loop.StartSample = existingLoop.StartSample;
                        this.Track.Loop.EndSample = existingLoop.EndSample;
                    }
                }
            })
            .DisposeWith(disposables);

            this._isLoopInputEnabled.DisposeWith(disposables);
            this._isDevMode.DisposeWith(disposables);
        });
    }

    public bool IsLoopInputEnabled => this._isLoopInputEnabled.Value;

    public bool IsDevMode => this._isDevMode.Value;

    public string SelectedInputFile
    {
        get => this._selectedInputFile;
        set => this.RaiseAndSetIfChanged(ref _selectedInputFile, value, nameof(SelectedInputFile));
    }

    public AudioTrack Track { get; }

    public ICommand CloseCommand { get; }

    public FileSelectInteraction ShowSelectFile { get; } = new();

    public Interaction<CreateTrackViewModel, Unit> EditTrack = new();

    public ObservableCollection<string> InputFileOptions { get; } = [NoInputFile];

    public string[] Encoders { get; }

    public ViewModelActivator Activator { get; } = new();

    [RelayCommand]
    private async Task SelectInputFile()
    {
        if (this.Track.Encoder == null)
        {
            return;
        }

        var encoder = this._encoders.GetEncoder(this.Track.Encoder);
        if (encoder == null)
        {
            return;
        }

        var filter = (encoder.InputTypes != null) ? $"Supported Types|{string.Join(';', encoder.InputTypes.Select(x => $"*{x}"))}" : null;
        var result = await this.ShowSelectFile.Handle(new()
        {
            Title = "Select Audio File",
            Filter = filter,
        });

        if (result.Length > 0)
        {
            var file = result[0];
            this.InputFileOptions.Add(file);
            this.SelectedInputFile = file;
        }
    }

    [RelayCommand]
    private async Task Edit()
    {
        await this.EditTrack.Handle(new(this.Track, this.project, this.Encoders));
    }
}
