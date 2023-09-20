using System.Text.RegularExpressions;

namespace bramvandenbussche.readwiser.domain.Model;

public static class DomainExtensions
{
    public static string FindChapterNumber(this string input)
    {
        var number = Regex.Match(input, @"\d+").Value;
        return string.IsNullOrEmpty(number) ? input : number;
    }
}