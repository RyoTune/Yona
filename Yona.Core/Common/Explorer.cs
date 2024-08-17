using System.Diagnostics;

namespace Yona.Core.Common;

internal static class Explorer
{
    public static void OpenDirectory(string dir)
    {
        try
        {
            if (Directory.Exists(dir))
            {
                Process.Start(new ProcessStartInfo() { UseShellExecute = true, FileName = dir });
            }
        }
        catch (Exception) { }
    }
}
