using Avalonia;
using Avalonia.Controls;
using System.Collections.Generic;
using Yona.Core.Audio.Models;

namespace Yona.Desktop.Controls;

public partial class AudioTracksList : UserControl
{
    public static readonly StyledProperty<IEnumerable<AudioTrack>?> TracksProperty =
        AvaloniaProperty.Register<AudioTracksList, IEnumerable<AudioTrack>?>(nameof(Tracks));

    public static readonly StyledProperty<AudioTrack?> SelectedTrackProperty =
        AvaloniaProperty.Register<AudioTracksList, AudioTrack?>(nameof(SelectedTrack), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public AudioTracksList()
    {
        InitializeComponent();
    }

    public IEnumerable<AudioTrack>? Tracks
    {
        get => this.GetValue(TracksProperty);
        set => this.SetValue(TracksProperty, value);
    }

    public AudioTrack? SelectedTrack
    {
        get => this.GetValue(SelectedTrackProperty);
        set => this.SetValue(SelectedTrackProperty, value);
    }
}