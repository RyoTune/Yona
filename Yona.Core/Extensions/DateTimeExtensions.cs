namespace Yona.Core.Extensions;

public static class DateTimeExtensions
{
    public static string ToPathSafe(this DateTime dateTime) => $"{dateTime:yyyyMMddTHHmmss}";
}
