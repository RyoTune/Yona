using VGAudio.Containers.Adx;
using VGAudio.Containers.Dsp;
using VGAudio.Containers.Hca;
using VGAudio.Containers.Hps;
using VGAudio.Containers.Idsp;
using VGAudio.Containers.NintendoWare;
using VGAudio.Containers.Wave;

namespace Yona.Core.Audio.Encoding.VGAudio;

internal static class ContainerTypes
{
    public static readonly Dictionary<FileType, ContainerType> Containers = new()
    {
        [FileType.Hca] = new ContainerType(new[] { "hca" }, () => new HcaReader(), () => new HcaWriter(), CreateConfiguration.Hca),
        [FileType.Adx] = new ContainerType(new[] { "adx" }, () => new AdxReader(), () => new AdxWriter(), CreateConfiguration.Adx),
        [FileType.Dsp] = new ContainerType(new[] { "dsp", "mdsp" }, () => new DspReader(), () => new DspWriter(), CreateConfiguration.Dsp),
        [FileType.Idsp] = new ContainerType(new[] { "idsp" }, () => new IdspReader(), () => new IdspWriter(), CreateConfiguration.Idsp),
        [FileType.Brstm] = new ContainerType(new[] { "brstm" }, () => new BrstmReader(), () => new BrstmWriter(), CreateConfiguration.Bxstm),
        [FileType.Bcstm] = new ContainerType(new[] { "bcstm" }, () => new BCFstmReader(), () => new BCFstmWriter(NwTarget.Ctr), CreateConfiguration.Bxstm),
        [FileType.Bfstm] = new ContainerType(new[] { "bfstm" }, () => new BCFstmReader(), () => new BCFstmWriter(NwTarget.Cafe), CreateConfiguration.Bxstm),
        [FileType.Hps] = new ContainerType(new[] { "hps" }, () => new HpsReader(), () => new HpsWriter(), CreateConfiguration.Hps),
        [FileType.Wave] = new ContainerType(new[] { "wav", "wave", "lwav" }, () => new WaveReader(), () => new WaveWriter(), CreateConfiguration.Wave),
    };

    public static readonly Dictionary<string, FileType> Extensions =
        Containers.SelectMany(x => x.Value.Names.Select(y => new { y, x.Key }))
        .ToDictionary(x => x.y, x => x.Key);

    public static readonly string[] ExtensionList = Extensions.Select(x => x.Key).ToArray();
}
