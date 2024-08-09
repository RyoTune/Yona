using IniParser;
using VGAudio.Codecs.CriAdx;
using VGAudio.Codecs.CriHca;
using VGAudio.Utilities;

namespace Yona.Core.Audio.Encoding.VGAudio;

/// <summary>
/// VGAudio config parser.
/// </summary>
internal static class ConfigParser
{
    private static readonly FileIniDataParser Parser = new();

    /// <summary>
    /// Parse a VGAudio INI config.
    /// </summary>
    /// <param name="file">Config file.</param>
    /// <returns>VGAudio config.</returns>
    public static VgAudioConfig Parse(string file)
    {
        var data = Parser.ReadFile(file);
        var config = new VgAudioConfig
        {
            Name = data.GetKey("name") ?? throw new Exception("Name is missing."),
            OutContainerFormat = data.GetKey("out_container_format")?.ToLower() ?? throw new Exception("Missing output container format."),
            OutFormat = data.GetKey("out_format")?.ToUpper(),
            LoopAlignment = int.TryParse(data.GetKey("loop_alignment"), out var loopAlignment) ? loopAlignment : default,
            BlockSize = int.TryParse(data.GetKey("block_size"), out var blockSize) ? blockSize : default,
            Version = int.TryParse(data.GetKey("version"), out var version) ? version : default,
            FrameSize = int.TryParse(data.GetKey("frame_size"), out var frameSize) ? frameSize : default,
            Filter = int.TryParse(data.GetKey("filter"), out var filter) ? filter : 2,
            AdxType = Enum.TryParse<CriAdxType>(data.GetKey("adx_type"), out var adxType) ? adxType : default,
            KeyString = data.GetKey("key_string"),
            KeyCode = ulong.TryParse(data.GetKey("key_code"), out var keyCode) ? keyCode : default,
            Endianness = Enum.TryParse<Endianness>(data.GetKey("endianess"), out var endianess) ? endianess : default,
            HcaQuality = Enum.TryParse<CriHcaQuality>(data.GetKey("hca_quality"), out var hcaQuality) ? hcaQuality : default,
            Bitrate = int.TryParse(data.GetKey("bitrate"), out var bitrate) ? bitrate : default,
            LimitBitrate = bool.TryParse(data.GetKey("limit_bitrate"), out var limitBitrate) && limitBitrate,
            EncodeCbr = bool.TryParse(data.GetKey("encode_cbr"), out var encodeCbr) && encodeCbr,
        };

        return config;
    }
}
