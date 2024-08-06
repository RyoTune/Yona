namespace Yona.Core.App;

public class AppService
{
    public AppService()
    {
        BaseDir = AppDomain.CurrentDomain.BaseDirectory;
    }

    public string BaseDir { get; init; }
}
