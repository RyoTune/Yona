using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.IO;
using System.Reactive.Disposables;
using Yona.Core.Projects.Models;
using Yona.Desktop.Common;

namespace Yona.Desktop.Controls;

public class ProjectIcon : ReactiveUserControl<ProjectBundle>
{
    private static readonly Uri appIconUri = new("avares://Yona.Desktop/Assets/Icons/logo-icon.webp");
    private static readonly Bitmap appIconSource = new(AssetLoader.Open(appIconUri));

    public ProjectIcon()
    {
        this.Content = new Image() { Source = appIconSource, Stretch = Avalonia.Media.Stretch.UniformToFill };
        this.WhenActivated((CompositeDisposable disposables) =>
        {
            this.ViewModel?.WhenPropertyChanged(x => x.IconFile)
            .Subscribe(change =>
            {
                this.Content = new Image() { Source = GetIconSource(change.Sender), Stretch = Avalonia.Media.Stretch.UniformToFill };
            })
            .DisposeWith(disposables);
        });
    }

    public static Bitmap GetIconSource(ProjectBundle project)
    {
        if (File.Exists(project.IconFile))
        {
            try
            {
                return new Bitmap(project.IconFile);
            }
            catch (Exception) { }
        }
        else if (IconUtils.GetDefaultIconFromName(project.Data.Name) is string knownName)
        {
            var iconUri = new Uri($"avares://Yona.Desktop/Assets/Icons/{knownName}.webp");

            try
            {
                return new Bitmap(AssetLoader.Open(iconUri));
            }
            catch (Exception) { }
        }

        return appIconSource;
    }
}
