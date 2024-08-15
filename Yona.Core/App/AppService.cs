namespace Yona.Core.App;

public class AppService
{
    public AppService()
    {
        this.BaseDir = AppDomain.CurrentDomain.BaseDirectory;
        this.AppDataDir = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Yona");
        Directory.CreateDirectory(this.AppDataDir);
    }

    public string BaseDir { get; }

    public string AppDataDir { get; }
}
