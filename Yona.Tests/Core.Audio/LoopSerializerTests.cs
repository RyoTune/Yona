using Yona.Core.Audio.Loops;

namespace Yona.Tests.Core.Audio;

public class LoopSerializerTests
{
    [Fact]
    public void LoopSerializer_Serialization()
    {
        var ogLoop = new Loop() { StartSample = 42, EndSample = 100, Enabled = true };
        var memStream = new MemoryStream();

        LoopSerializer.Serialize(memStream, ogLoop);
        var newLoop = LoopSerializer.Deserialize(new MemoryStream(memStream.ToArray()));

        Assert.Equal(ogLoop.Enabled, newLoop.Enabled);
        Assert.Equal(ogLoop.StartSample, newLoop.StartSample);
        Assert.Equal(ogLoop.EndSample, newLoop.EndSample);
    }
}
