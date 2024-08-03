using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using Yona.Library.Common;
using Yona.Library.Common.Serializers;
using Yona.Library.Settings.Models;

namespace Yona.Library.Settings;

public class SettingsService : ObservableObject
{
    private readonly ILogger log;
    private readonly SavableFile<AppSettings> settings;

    public SettingsService(ILogger log)
    {
        this.log = log;

        var appDir = AppDomain.CurrentDomain.BaseDirectory;
        var settingsFile = Path.Join(appDir, "settings.json");
        this.settings = new SavableFile<AppSettings>(settingsFile, JsonFileSerializer.Instance);

        this.Current.PropertyChanged += OnSettingsChanged;
    }

    public AppSettings Current => this.settings.Value;

    public void Reset()
    {
        this.Current.PropertyChanged -= this.OnSettingsChanged;

        this.settings.Value = new();
        this.settings.Save();
        this.OnPropertyChanged(nameof(this.Current));

        this.Current.PropertyChanged += this.OnSettingsChanged;
    }

    private void OnSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        try
        {
            this.settings.Save();
            this.log.LogDebug("Saved settings.");
        }
        catch (Exception ex)
        {
            this.log.LogError(ex, "Failed to auto-save settings.");
        }
    }
}
