using VGAudio.Containers;

namespace Yona.Core.Audio.Encoding.VGAudio;

internal class ContainerType
{
    public ContainerType(IEnumerable<string> names, Func<IAudioReader> getReader, Func<IAudioWriter> getWriter, Func<VgAudioConfig, Configuration> getConfiguration)
    {
        Names = names;
        GetReader = getReader;
        GetWriter = getWriter;
        GetConfiguration = getConfiguration;
    }

    public IEnumerable<string> Names { get; }

    public Func<IAudioReader> GetReader { get; }

    public Func<IAudioWriter> GetWriter { get; }

    public Func<VgAudioConfig, Configuration> GetConfiguration { get; }
}
