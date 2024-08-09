using VGAudio.Containers;
using Yona.Core.Audio.Loops;

namespace Yona.Core.Audio.Encoding.VGAudio;

public class VgAudioEncoder : IEncoder
{
    private readonly ContainerType outputContainer;
    private readonly IAudioWriter writer;
    private readonly Configuration? configuration;

    public VgAudioEncoder(Config config)
    {
        if (config.OutContainerFormat == null)
        {
            throw new ArgumentException("Config missing output container format.");
        }

        outputContainer = ContainerTypes.Containers.First(container => container.Value.Names.Contains(config.OutContainerFormat, StringComparer.OrdinalIgnoreCase)).Value;
        writer = outputContainer.GetWriter();
        configuration = outputContainer.GetConfiguration(config);
        EncodedExt = $".{config.OutContainerFormat}";
    }

    public string EncodedExt { get; }

    public string[] InputTypes { get; } = ContainerTypes.ExtensionList.Select(x => $".{x}").ToArray();

    public Task Encode(string inputFile, string outputFile, Loop loop)
    {
        return Task.Run(() =>
        {
            var inputFileExt = Path.GetExtension(inputFile).Trim('.');

            using var inputStream = File.OpenRead(inputFile);
            using var outputStream = File.Create(outputFile);

            var readerContainer = ContainerTypes.Containers.First(x => x.Value.Names.Contains(inputFileExt, StringComparer.OrdinalIgnoreCase)).Value;
            var reader = readerContainer.GetReader();
            var inputAudio = reader.Read(inputStream);

            try
            {
                if (loop.FullLoop)
                {
                    inputAudio.SetLoop(loop.Enabled);
                }
                else if (loop.Enabled)
                {
                    if (loop.StartSample > loop.EndSample)
                    {
                        throw new Exception("Loop start sample can't be past loop end sample.");
                    }
                    else
                    {
                        inputAudio.SetLoop(loop.Enabled, loop.StartSample, loop.EndSample);
                    }
                }
            }
            catch (Exception)
            {
            }

            writer.WriteToStream(inputAudio, outputStream, configuration);
        });
    }
}
