using System.Text.RegularExpressions;

namespace bramvandenbussche.readwiser.api.Domain;

public static class DomainExtensions
{
    public static string FindChapterNumber(this string input)
    {
        var number = Regex.Match(input, @"\d+").Value;
        if (string.IsNullOrEmpty(number)) return input;
        return number;
    }
}