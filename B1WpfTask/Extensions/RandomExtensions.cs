using System.Text;

namespace B1WPFTask.Extensions;
public static class RandomExtensions
{
    private const string _latinAlphabet = "abcdefghijklmnopqrstuvwxyz";
    private const string _russianAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

    public static DateTime DateInLastYears(this Random random, int years)
    {
        if (years <= 0)
        {
            throw new ArgumentException("Should be positive", nameof(years));
        }

        var randomYear = random.Next(years);
        var randomMonth = random.Next(12);
        var daysInMonth = DateTime.DaysInMonth(DateTime.UtcNow.Year - randomYear, DateTime.UtcNow.AddMonths(-randomMonth).Month);
        var randomDay = random.Next(daysInMonth);

        return DateTime.UtcNow
            .AddDays(-randomDay)
            .AddMonths(-randomMonth)
            .AddYears(-randomYear);
    }

    public static int PositiveIntInRange(this Random random, int start, int end)
    {
        var randomNumber = random.Next(start, end);
        return randomNumber.IsOdd() ? randomNumber + 1 : randomNumber;
    }

    public static int PositiveIntInRange(this Random random, Range range) =>
        random.PositiveIntInRange(range.Start.Value, range.End.Value);

    public static string LatinString(this Random random, int length) =>
        random.StringFromSymbolsWithLength(length, _latinAlphabet);

    public static string RussianString(this Random random, int length) =>
        random.StringFromSymbolsWithLength(length, _russianAlphabet);

    public static string StringFromSymbolsWithLength(this Random random, int length, string symbols)
    {
        if (length <= 0)
        {
            throw new ArgumentException("Should be positive", nameof(length));
        }

        ArgumentException.ThrowIfNullOrEmpty(symbols, nameof(symbols));

        var builder = new StringBuilder(length);
        while (length --> 0)
        {
            var isUpper = random.Bool();
            var randomChar = random.CharFromString(symbols);
            builder.Append(isUpper ? char.ToUpper(randomChar) : randomChar);
        }

        return builder.ToString();
    }

    public static double PositiveDoubleInRange(this Random random, int start, int end, int decimalPlaces)
    {
        var multiplier = (int)Math.Pow(10, decimalPlaces);
        return (double)random.Next(start * multiplier, end * multiplier + 1) / multiplier;
    }

    public static double PositiveDoubleInRange(this Random random, Range range, int decimalPlaces) =>
        random.PositiveDoubleInRange(range.Start.Value, range.End.Value, decimalPlaces);

    public static bool Bool(this Random random) =>
        Convert.ToBoolean(random.Next(2));

    public static char CharFromString(this Random random, string str) =>
        str[random.Next(str.Length)];
}
