using CommunityToolkit.Mvvm.ComponentModel;
using Yona.Core.Audio.Loops;

namespace Yona.Core.Audio.Models;

public partial class ObservableLoop : ObservableObject
{
    private readonly Loop loop = new();

    public ObservableLoop()
    {
    }

    public ObservableLoop(Loop loop)
    {
        this.loop = loop;
    }

    public bool Enabled
    {
        get => this.loop.Enabled;
        set => this.SetProperty(this.Enabled, value, this.loop, (m, n) => m.Enabled = n);
    }

    public int StartSample
    {
        get => this.loop.StartSample;
        set => this.SetProperty(this.loop.StartSample, value, this.loop, (m, n) => m.StartSample = n);
    }

    public int EndSample
    {
        get => this.loop.EndSample;
        set => this.SetProperty(this.loop.EndSample, value, this.loop, (m, n) => m.EndSample = n);
    }
}

public static class LoopExtensions
{
    public static Loop ToModel(this ObservableLoop loop) => new() { Enabled = loop.Enabled, StartSample = loop.StartSample, EndSample = loop.EndSample };
}
