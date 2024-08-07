using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Yona.Core.Projects.Models;

namespace Yona.Core.ViewModels.TrackPanel;

public partial class TrackPanelViewModel : ViewModelBase
{
    private const string NoReplacement = "None";

    //private readonly AudioService audioManager;
    //private readonly LoopService loopService;
    //private readonly AudioEncoderRegistry encoderRegistry;
    //private readonly IDialogService dialog;

    private string selectedReplacement;

    public TrackPanelViewModel(
        AudioTrack track,
        ICommand closeCommand)
    {
        //this.audioManager = audioManager;
        //this.loopService = loopService;
        //this.encoderRegistry = encoderRegistry;
        //this.dialog = dialog;

        this.Track = track;
        //this.Encoders = encoderRegistry.Encoders.Keys.ToArray();
        this.CloseCommand = closeCommand;

        // Set current replacement selection.
        //if (track.ReplacementFile != null)
        //{
        //    this.Replacements.Add(track.ReplacementFile);
        //    this.selectedReplacement = track.ReplacementFile;
        //}
        //else
        //{
        //    this.selectedReplacement = NoReplacement;
        //}

        // Save tracks on changes made.
        //this.Track.PropertyChanged += this.Track_PropertyChanged;
        //this.Track.Loop.PropertyChanged += this.Track_PropertyChanged;
        //this.Track.Loop.PropertyChanged += this.Loop_PropertyChanged;
    }

    public AudioTrack Track { get; init; }

    public ICommand CloseCommand { get; init; }

    public bool LoopInputEnabled => this.Track.InputFile != null /*&& this.Track.Loop.Enabled*/;

    public ObservableCollection<string> Replacements { get; } = new() { NoReplacement };

    public string SelectedReplacement
    {
        get => this.selectedReplacement;
        set
        {
            this.SetProperty(ref this.selectedReplacement, value);

            // Push selection to track.
            if (this.selectedReplacement == NoReplacement)
            {
                this.Track.InputFile = null;
            }
            else
            {
                this.Track.InputFile = this.selectedReplacement;
            }
        }
    }

    public string[] Encoders { get; }

    public void Dispose()
    {
        this.Track.PropertyChanged -= this.Track_PropertyChanged;
        //this.Track.Loop.PropertyChanged -= this.Track_PropertyChanged;
        //this.Track.Loop.PropertyChanged -= this.Loop_PropertyChanged;
        GC.SuppressFinalize(this);
    }

    [RelayCommand]
    private async Task SelectReplacementFile()
    {
        if (this.Track.Encoder == null)
        {
            return;
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

    private void Track_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Save tracks on changes.
        //this.audioManager.SaveTracks();

        //// Update loop input enabled state.
        //if (e.PropertyName == nameof(this.Track.ReplacementFile) || e.PropertyName == nameof(this.Track.Loop.Enabled))
        //{
        //    this.OnPropertyChanged(nameof(this.LoopInputEnabled));
        //}
    }

    private void Loop_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Save loop settings for replacement file on changes.
        //if (this.Track.ReplacementFile != null)
        //{
        //    this.loopService.SaveLoop(this.Track.ReplacementFile, this.Track.Loop);
        //}
    }
}
