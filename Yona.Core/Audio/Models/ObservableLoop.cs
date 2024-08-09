using CommunityToolkit.Mvvm.ComponentModel;
using Yona.Core.Audio.Loops;

namespace Yona.Core.Audio.Models;

public partial class ObservableLoop : ObservableObject
{
    public ObservableLoop()
    {
    }

    public ObservableLoop(Loop loop)
    {
        this.Model = loop;
    }

    public Loop Model { get; } = new();

    public bool Enabled
    {
        get => this.Model.Enabled;
        set => this.SetProperty(this.Enabled, value, this.Model, (m, n) => m.Enabled = n);
    }

    public int StartSample
    {
        get => this.Model.StartSample;
        set => this.SetProperty(this.Model.StartSample, value, this.Model, (m, n) => m.StartSample = n);
    }

    public int EndSample
    {
        get => this.Model.EndSample;
        set => this.SetProperty(this.Model.EndSample, value, this.Model, (m, n) => m.EndSample = n);
    }
}
