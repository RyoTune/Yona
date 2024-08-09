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

namespace Yona.Core.ViewModels.TrackPanel;

public partial class TrackPanelViewModel : ViewModelBase, IActivatableViewModel
{
    private const string NoInputFile = "None";

    private readonly LoopService loops;
    private readonly EncoderRepository encoders;

    private AudioTrack _track;
    private string _selectedInputFile;

    private readonly ObservableAsPropertyHelper<bool> _isLoopInputEnabled;
    private readonly ObservableAsPropertyHelper<bool> _devModeEnabled;

    public bool IsLoopInputEnabled => this._isLoopInputEnabled.Value;

    public bool DevModeEnabled => this._devModeEnabled.Value;

    public TrackPanelViewModel(
        AudioTrack track,
        LoopService loops,
        EncoderRepository encoders,
        SettingsService settings,
        ICommand saveProjectCommand,
        ICommand closeCommand)
    {
        //this.audioManager = audioManager;

        this.loops = loops;
        this.encoders = encoders;
        this.CloseCommand = closeCommand;
        this._track = track;

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

        this._devModeEnabled = settings.WhenAnyValue(x => x.Current.DevModeEnabled).ToProperty(this, x => x.DevModeEnabled);

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            var trackObs = track.WhenAnyPropertyChanged();
            var loopObs = track.Loop.WhenAnyPropertyChanged();

            // Save loop.
            loopObs.Throttle(TimeSpan.FromMilliseconds(250))
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
            .Throttle(TimeSpan.FromMilliseconds(250))
            .Subscribe(values => saveProjectCommand?.Execute(null))
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

                    var existingLoop = this.loops.GetLoop(file);
                    if (existingLoop != null)
                    {
                        if (existingLoop.StartSample != 0 || existingLoop.EndSample != 0)
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
            this._devModeEnabled.DisposeWith(disposables);
        });
    }

    public string SelectedInputFile
    {
        get => this._selectedInputFile;
        set => this.RaiseAndSetIfChanged(ref _selectedInputFile, value, nameof(SelectedInputFile));
    }

    public AudioTrack Track
    {
        get => this._track;
        init => this.RaiseAndSetIfChanged(ref this._track, value);
    }

    public ICommand CloseCommand { get; }

    public Interaction<FileSelectOptions, string[]> ShowSelectFile { get; } = new();

    public ObservableCollection<string> InputFileOptions { get; } = [NoInputFile];

    public string[] Encoders => this.encoders.Items.Select(x => x.Name).ToArray();

    public ViewModelActivator Activator { get; } = new();

    [RelayCommand]
    private async Task SelectInputFile()
    {
        if (this.Track.Encoder == null)
        {
            return;
        }

        var result = await this.ShowSelectFile.Handle(new()
        {
            Title = "Select Audio File...",
        });

        if (result.Length > 0)
        {
            var file = result[0];
            this.InputFileOptions.Add(file);
            this.SelectedInputFile = file;
        }

        //if (this.encoderRegistry.Encoders.TryGetValue(this.Track.Encoder, out var encoder))
        //{
        //    var inputTypes = this.encoderRegistry.Encoders[this.Track.Encoder].InputTypes;
        //    var fileFilter = $"Supported Types|{string.Join(';', inputTypes.Select(x => $"*{x}"))}";
        //    var replacementFile = await this.dialog.OpenFileSelect("Select Replacement File...", fileFilter);
        //    if (replacementFile != null)
        //    {
        //        var savedLoop = this.loopService.GetLoop(replacementFile);
        //        if (savedLoop != null)
        //        {
        //            this.Track.Loop.Enabled = savedLoop.Enabled;
        //            this.Track.Loop.StartSample = savedLoop.StartSample;
        //            this.Track.Loop.EndSample = savedLoop.EndSample;
        //        }
        //        else
        //        {
        //            this.Track.Loop.Enabled = true;
        //            this.Track.Loop.StartSample = 0;
        //            this.Track.Loop.EndSample = 0;
        //        }

        //        this.Replacements.Add(replacementFile);
        //        this.SelectedReplacement = replacementFile;
        //    }
        //}
    }

    [RelayCommand]
    private async Task Edit()
    {
        //var editTrack = new AddTrackViewModel(this.Encoders, this.Track);
        //var updatedTrack = await this.dialog.OpenDialog<AudioTrack>(editTrack);

        //if (updatedTrack != null)
        //{
        //    this.Track.Name = updatedTrack.Name;
        //    this.Track.Category = updatedTrack.Category;
        //    this.Track.Tags = updatedTrack.Tags;
        //    this.Track.OutputPath = updatedTrack.OutputPath;
        //    this.Track.Encoder = updatedTrack.Encoder;
        //}
    }

    [RelayCommand]
    private void Delete()
    {
        //this.audioManager.RemoveTrack(this.Track);
    }
}
