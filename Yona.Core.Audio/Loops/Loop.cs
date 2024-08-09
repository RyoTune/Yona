namespace Yona.Core.Audio.Loops;

public class Loop
{
    public bool Enabled { get; set; }

    public int StartSample { get; set; }

    public int EndSample { get; set; }

    public bool FullLoop => this.Enabled && this.StartSample == 0 && this.EndSample == 0;
}
