﻿using Microsoft.Extensions.Logging;
using VGAudio.Containers;
using Yona.Core.Audio.Loops;

namespace Yona.Core.Audio.Encoding.VGAudio;

public class VgAudioEncoder : IEncoder
{
    private readonly ContainerType outputContainer;
    private readonly IAudioWriter writer;
    private readonly Configuration? configuration;
    private readonly ILogger? log;

    public VgAudioEncoder(ILogger? log, string configFile)
        : this(log, ConfigParser.Parse(configFile))
    {
    }

    public VgAudioEncoder(ILogger? log, VgAudioConfig config)
    {
        this.log = log;
        if (config.OutContainerFormat == null)
        {
            throw new ArgumentException("Config missing output container format.");
        }

        this.Name = config.Name!;
        this.outputContainer = ContainerTypes.Containers.First(container => container.Value.Names.Contains(config.OutContainerFormat, StringComparer.OrdinalIgnoreCase)).Value;
        this.writer = outputContainer.GetWriter();
        this.configuration = outputContainer.GetConfiguration(config);
        this.EncodedExt = $".{config.OutContainerFormat}";
    }

    public string Name { get; }

    public string EncodedExt { get; }

    public string[] InputTypes { get; } = ContainerTypes.ExtensionList.Select(x => $".{x}").ToArray();

    public Task Encode(string inputFile, string outputFile, Loop? loop)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(outputFile)!);
        return Task.Run(() =>
        {
            var inputFileExt = Path.GetExtension(inputFile).Trim('.');

            using var inputStream = File.OpenRead(inputFile);
            using var outputStream = File.Create(outputFile);

            var readerContainer = ContainerTypes.Containers.First(x => x.Value.Names.Contains(inputFileExt, StringComparer.OrdinalIgnoreCase)).Value;
            var reader = readerContainer.GetReader();
            var inputAudio = reader.Read(inputStream);

            if (loop?.Enabled == true)
            {
                try
                {
                    if (loop.FullLoop)
                    {
                        inputAudio.SetLoop(loop.Enabled);
                    }
                    else if (loop.StartSample > loop.EndSample)
                    {
                        throw new Exception("Loop start sample can't be past loop end sample.");
                    }
                    else
                    {
                        inputAudio.SetLoop(loop.Enabled, loop.StartSample, loop.EndSample);
                    }
                }
                catch (Exception ex)
                {
                    log?.LogWarning(ex, "Possible error while looping file, ignore if file works as expected.\nFile: {file}", inputFile);
                }
            }

            writer.WriteToStream(inputAudio, outputStream, configuration);
        });
    }
}
