using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using Yona.Core.Common;
using Yona.Core.Settings.Models;
using Yona.Core.Utils.Serializers;

namespace Yona.Core.Settings;

public class SettingsService : ObservableObject
{
    private readonly ILogger log;
    private readonly SavableFile<AppSettings> settings;

    public SettingsService(ILogger log)
    {
        this.log = log;

        var appDir = AppDomain.CurrentDomain.BaseDirectory;
        var settingsFile = Path.Join(appDir, "settings.json");
        settings = new SavableFile<AppSettings>(settingsFile, JsonFileSerializer.Instance);

        Current.PropertyChanged += OnSettingsChanged;
    }

    public AppSettings Current => settings.Data;

    public void Reset()
    {
        Current.PropertyChanged -= OnSettingsChanged;

        settings.Data = new();
        settings.Save();
        OnPropertyChanged(nameof(Current));

        Current.PropertyChanged += OnSettingsChanged;
    }

    private void OnSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        try
        {
            settings.Save();
            this.log.LogDebug("Settings saved.");
        }
        catch (Exception ex)
        {
            this.log.LogError(ex, "Failed to auto-save settings.");
        }
    }
}
