using System.Security.Cryptography;

namespace Yona.Core.Utils;

public static class Checksum
{
    public static string Compute(string inputFile)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(inputFile);
        var hash = md5.ComputeHash(stream);
        return Convert.ToHexString(hash);
    }
}