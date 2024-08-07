namespace Yona.Core.App;

public class AppService
{
    public AppService()
    {
        this.BaseDir = AppDomain.CurrentDomain.BaseDirectory;
    }

    public string BaseDir { get; init; }
}
