using ReactiveUI;
using Yona.Core.App;
using Yona.Core.Common;
using Yona.Core.Extensions;
using Yona.Core.Settings.Models;
using Yona.Core.Utils.Serializers;

namespace Yona.Core.Settings;

public class SettingsService : ReactiveObject
{
    private readonly SavableFile<AppSettings> settings;

    private IDisposable? disposable;

    public SettingsService(AppService app)
    {
        this.settings = new SavableFile<AppSettings>(Path.Join(app.AppDataDir, "settings.json"), JsonFileSerializer.Instance);
        this.LogsDir = Path.Join(app.AppDataDir, "logs");
        Directory.CreateDirectory(this.LogsDir);

        this.WhenAnyValue(x => x.Current)
            .Subscribe(_ =>
            {
                this.disposable?.Dispose();
                this.disposable = this.settings.AutosaveWithChanges();
            });
    }

    public string LogsDir { get; }

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
