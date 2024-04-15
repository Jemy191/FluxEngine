namespace Flux.Core;

public static class StringExtensions
{
    public static string JoinString<T>(this IEnumerable<T> values, char separator) => string.Join(separator, values);
    public static string JoinString<T>(this IEnumerable<T> values, string separator) => string.Join(separator, values);
}