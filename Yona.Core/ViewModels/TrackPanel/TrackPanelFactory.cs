using System.Windows.Input;
using Yona.Core.Audio;
using Yona.Core.Audio.Models;
using Yona.Core.Settings;

namespace Yona.Core.ViewModels.TrackPanel;

public class TrackPanelFactory(EncoderRepository encoders, LoopService loops, SettingsService settings)
{
    private readonly EncoderRepository encoders = encoders;
    private readonly LoopService loops = loops;
    private readonly SettingsService settings = settings;

    public TrackPanelViewModel Create(AudioTrack track, ICommand saveProjectCommand, ICommand closeCommand)
        => new(track, this.loops, this.encoders, this.settings, saveProjectCommand, closeCommand);
}
