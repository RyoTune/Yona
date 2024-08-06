namespace Yona.Core.Settings.Models;

public enum Page
{
    Home,
    Projects,
    Convert,

#if DEBUG
    Settings,
#endif
}
