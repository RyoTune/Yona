using VGAudio.Codecs.CriAdx;
using VGAudio.Codecs.CriHca;
using VGAudio.Utilities;

namespace Yona.Core.Audio.Encoding.VGAudio;

public class Config
{
    public string? Name { get; set; }

    public string? OutContainerFormat { get; set; }

    public string? OutFormat { get; set; }

    public int LoopAlignment { get; set; }

    public int BlockSize { get; set; }

    public int Version { get; set; } // ADX

    public int FrameSize { get; set; } // ADX

    public int Filter { get; set; } = 2; // ADX

    public CriAdxType AdxType { get; set; } // ADX

    public string? KeyString { get; set; } // ADX

    public ulong KeyCode { get; set; } // ADX

    public Endianness? Endianness { get; set; }

    public CriHcaQuality HcaQuality { get; set; }

    public int Bitrate { get; set; }

    public bool LimitBitrate { get; set; }

    public bool EncodeCbr { get; set; }
}
