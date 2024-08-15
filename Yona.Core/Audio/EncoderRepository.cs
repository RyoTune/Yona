using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Yona.Core.App;
using Yona.Core.Audio.Encoding;
using Yona.Core.Audio.Encoding.VGAudio;

namespace Yona.Core.Audio;

public class EncoderRepository
{
    private readonly string encodersDir;
    private readonly string cachedDir;
    private readonly ILogger log;

    public EncoderRepository(AppService app, ILogger<EncoderRepository> log)
    {
        this.log = log;

        this.encodersDir = Path.Join(app.BaseDir, "audio", "encoders");
        this.cachedDir = Path.Join(app.BaseDir, "audio", "cached");

        Directory.CreateDirectory(this.encodersDir);
        Directory.CreateDirectory(this.cachedDir);

        this.LoadEncoders();
    }

    public ObservableCollection<IEncoder> Items { get; } = [];

    public string[] AvailableEncoders => this.Items.Select(x => x.Name).ToArray();

    public IEncoder? GetEncoder(string name) => this.Items.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    private void LoadEncoders()
    {
        this.LoadVgaudioEncoders();
        this.LoadDefaultEncoders();
    }

    private void LoadDefaultEncoders()
    {
        var copyEncoder = new CopyEncoder();
        this.Items.Add(copyEncoder);

        foreach (var container in ContainerTypes.Containers)
        {
            try
            {
                var type = container.Value;
                var name = type.Names.First();
                var encoder = new VgAudioEncoder(new VgAudioConfig() { Name = name.ToUpper(), OutContainerFormat = name });
                this.Items.Add(this.CreateCachedEncoder(encoder));
            }
            catch (Exception ex)
            {
                this.log?.LogError(ex, "Failed to create default encoder.");
            }
        }
    }

    private void LoadVgaudioEncoders()
    {
        var vgAudioDir = Path.Join(this.encodersDir, "VGAudio");
        if (!Directory.Exists(vgAudioDir))
        {
            return;
        }

        foreach (var configFile in Directory.EnumerateFiles(vgAudioDir, "*.ini", SearchOption.AllDirectories))
        {
            try
            {
                var encoder = new VgAudioEncoder(configFile);
                this.Items.Add(this.CreateCachedEncoder(encoder));
            }
            catch (Exception ex)
            {
                this.log.LogError(ex, "Failed to load VGAudio INI.\nFile: {configFile}", configFile);
            }
        }
    }

    private CachedEncoder CreateCachedEncoder(IEncoder encoder)
    {
        var encoderCachedDir = Path.Join(this.cachedDir, encoder.Name);
        Directory.CreateDirectory(encoderCachedDir);

        return new CachedEncoder(encoder, encoderCachedDir, this.log);
    }
}
