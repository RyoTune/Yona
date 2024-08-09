namespace Yona.Core.Audio.Loops;

public static class LoopSerializer
{
    public static void SerializeFile(string loopFile, Loop loop)
    {
        using var fs = File.OpenWrite(loopFile);
        Serialize(fs, loop);
    }

    public static Loop DeserializeFile(string loopFile)
    {
        using var fs = File.OpenRead(loopFile);
        return Deserialize(fs);
    }

    public static void Serialize(Stream stream, Loop loop)
    {
        using var bw = new BinaryWriter(stream);
        bw.Write(loop.Enabled);
        bw.Write(loop.StartSample);
        bw.Write(loop.EndSample);
    }

    public static Loop Deserialize(Stream stream)
    {
        using var br = new BinaryReader(stream);
        var loop = new Loop()
        {
            Enabled = br.ReadBoolean(),
            StartSample = br.ReadInt32(),
            EndSample = br.ReadInt32(),
        };

        return loop;
    }
}
