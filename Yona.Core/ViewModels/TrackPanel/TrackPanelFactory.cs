using System.Windows.Input;
using Yona.Core.Audio;
using Yona.Core.Audio.Models;

namespace Yona.Core.ViewModels.TrackPanel;

public class TrackPanelFactory(LoopService loops, EncoderRepository encoders)
{
    private readonly LoopService loops = loops;
    private readonly EncoderRepository encoders = encoders;

    public TrackPanelViewModel Create(AudioTrack track, ICommand saveProjectCommand, ICommand closeCommand)
        => new(track, this.loops, this.encoders, saveProjectCommand, closeCommand);
}
