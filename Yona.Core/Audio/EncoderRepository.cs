using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Yona.Core.App;
using Yona.Core.Audio.Encoding;
using Yona.Core.Audio.Encoding.VGAudio;

namespace Yona.Core.Audio;

public class EncoderRepository
{
    private readonly string encodersDir;
    private readonly ILogger log;

    public EncoderRepository(AppService app, ILogger log)
    {
        this.log = log;

        this.encodersDir = Path.Join(app.BaseDir, "audio", "encoders");
        Directory.CreateDirectory(encodersDir);

        this.LoadEncoders();
    }

    public ObservableCollection<IEncoder> Items { get; } = [];

    private void LoadEncoders()
    {
        this.LoadVgaudioEncoders();
    }

    private void LoadVgaudioEncoders()
    {
        var vgAudioDir = Path.Join(this.encodersDir, "vgaudio");
        if (!Directory.Exists(vgAudioDir))
        {
            return;
        }

        foreach (var configFile in Directory.EnumerateFiles(vgAudioDir, "*.ini", SearchOption.AllDirectories))
        {
            try
            {
                var encoder = new VgAudioEncoder(configFile);
                this.Items.Add(encoder);
            }
            catch (Exception ex)
            {
                this.log.LogError(ex, "Failed to load VGAudio INI.\nFile: {configFile}", configFile);
            }
        }
    }
}
