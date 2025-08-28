using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Reactive.Linq;
using Yona.Core.Common.Dialog;
using Yona.Core.ViewModels.Convert;

namespace Yona.Core.ViewModels.Dashboard.Convert;

public partial class DashboardConvertViewModel(ConvertFactory convert) : ViewModelBase
{
    [ObservableProperty]
    private ConvertViewModel? _currentConvert;

    public FolderSelectInteraction FolderSelect { get; } = new();

    public FileSelectInteraction FileSelect { get; } = new();

    [RelayCommand]
    private async Task SelectFolder()
    {
        var folders = await this.FolderSelect.Handle(new() { AllowMultiple = true });
        if (folders.Length == 0)
        {
            return;
        }

        var files = folders.SelectMany(x => Directory.GetFiles(x, "*", SearchOption.AllDirectories)).ToArray();
        this.CurrentConvert = convert.Create(files);
        this.CurrentConvert.OutputFolder = FolderSelect;
    }

    [RelayCommand]
    private async Task SelectFile()
    {
        var files = await this.FileSelect.Handle(new() { AllowMultiple = true });
        if (files.Length == 0)
        {
            return;
        }

        this.CurrentConvert = convert.Create(files);
        this.CurrentConvert.OutputFolder = FolderSelect;
    }
}
