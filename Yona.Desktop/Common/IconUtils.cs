using Yona.Core.Common;

namespace Yona.Desktop.Common;

internal static class IconUtils
{
    public static string? GetDefaultIconFromName(string name)
    {
        if (GetGameName(name) is string gameName)
        {
            var gameIconName = gameName.ToLower().Replace(" ", "-");
            return gameIconName;
        }

        return null;
    }

    private static string? GetGameName(string name)
    {
        if (name.Contains(GameName.P5R, System.StringComparison.OrdinalIgnoreCase))
        {
            return GameName.P5R;
        }

        if (name.Contains(GameName.P4G, System.StringComparison.OrdinalIgnoreCase))
        {
            return GameName.P4G;
        }

        if (name.Contains(GameName.P3R, System.StringComparison.OrdinalIgnoreCase))
        {
            return GameName.P3R;
        }

        if (name.Contains(GameName.P3P, System.StringComparison.OrdinalIgnoreCase))
        {
            return GameName.P3P;
        }

        return null;
    }
}
