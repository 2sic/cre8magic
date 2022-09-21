using System.Diagnostics.CodeAnalysis;
using static System.StringComparison;

namespace ToSic.Cre8Magic.Client;

public static class StringExtensions
{
    internal static bool EqInvariant(this string? a, string? b)
        => a == null && b == null
           || string.Equals(a, b, InvariantCultureIgnoreCase);

    internal static bool HasValue([NotNullWhen(true)] this string? value)
        => !string.IsNullOrEmpty(value);

    internal static bool HasText([NotNullWhen(true)] this string? value)
        => !string.IsNullOrWhiteSpace(value);

    internal static string? EmptyAsNull(this string? value) => string.IsNullOrWhiteSpace(value) ? null : value;

    internal static string? Replace(this string? value, string oldValue, Func<string> newValueGenerator)
    {
        if (!value.HasValue() || !value!.Contains(oldValue) || !oldValue.HasValue())
            return value;
        var newValue = newValueGenerator();
        return value.Replace(oldValue, newValue);
    }
}