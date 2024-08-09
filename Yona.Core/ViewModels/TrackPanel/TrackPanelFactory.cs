using System.Windows.Input;
using Yona.Core.Audio;
using Yona.Core.Audio.Models;
using Yona.Core.Settings;

namespace Yona.Core.ViewModels.TrackPanel;

public class TrackPanelFactory(SettingsService settings, LoopService loops, EncoderRepository encoders)
{
    private readonly SettingsService settings = settings;
    private readonly LoopService loops = loops;
    private readonly EncoderRepository encoders = encoders;

    public TrackPanelViewModel Create(AudioTrack track, ICommand saveProjectCommand, ICommand closeCommand)
        => new(track, this.loops, this.encoders, this.settings, saveProjectCommand, closeCommand);
}
