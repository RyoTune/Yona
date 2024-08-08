using CommunityToolkit.Mvvm.ComponentModel;
    
namespace Yona.Core.Audio.Models;

public partial class Loop : ObservableObject
{
    [ObservableProperty]
    private bool enabled;

    [ObservableProperty]
    private int startSample;

    [ObservableProperty]
    private int endSample;
}
