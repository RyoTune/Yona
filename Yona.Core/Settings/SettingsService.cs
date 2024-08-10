using Microsoft.Extensions.Logging;
using ReactiveUI;
using Yona.Core.Common;
using Yona.Core.Extensions;
using Yona.Core.Settings.Models;
using Yona.Core.Utils.Serializers;

namespace Yona.Core.Settings;

public class SettingsService : ReactiveObject
{
    private readonly ILogger log;
    private readonly SavableFile<AppSettings> settings;

    private IDisposable? disposable;

    public SettingsService(ILogger log)
    {
        this.log = log;

        var appDir = AppDomain.CurrentDomain.BaseDirectory;
        var settingsFile = Path.Join(appDir, "settings.json");

        this.settings = new SavableFile<AppSettings>(settingsFile, JsonFileSerializer.Instance);

        this.WhenAnyValue(x => x.Current)
            .Subscribe(_ =>
            {
                this.disposable?.Dispose();
                this.disposable = this.settings.AutosaveWithChanges();
            });
    }

    public AppSettings Current
    {
        get => this.settings.Data;
        set
        {
            this.RaisePropertyChanging(nameof(this.Current));
            this.settings.Data = value;
            this.RaisePropertyChanged(nameof(this.Current));
        }
    }

    public void Reset()
    {
        this.Current = new();
        this.settings.Save();
    }
}
