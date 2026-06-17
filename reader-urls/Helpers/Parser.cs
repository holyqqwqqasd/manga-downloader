using System.Text.RegularExpressions;
using reader_urls.Models;

namespace reader_urls.Helpers;

public static partial class Parser
{
    public static List<ParsedChapter> ParseChapterLinks(string html)
    {
        var d = new Dictionary<string, string>();

        // Находим все совпадения
        var matches = ChapterLinksRegex().Matches(html);

        foreach (Match match in matches)
        {
            var link = match.Groups[1].Value;
            var text = match.Groups[2].Value;
            d.Add(text.Trim(), link);
        }

        return d.Reverse()
                .Select((x, i) => new ParsedChapter(i, x.Key, x.Value))
                .ToList();
    }

    public static List<string> ParseImagesFromChapter(string html)
    {
        var result = new List<string>();

        // Находим все совпадения
        var matches = ImagesFromChapterRegex().Matches(html);

        foreach (Match match in matches)
        {
            var array = match.Groups[1].Value;
            var images = array[2..^2].Split("],[");

            foreach (var i in images)
            {
                var parts = i.Split(",");
                var baseUrl = parts[0][1..^1];
                var pathUrl = parts[2][1..^1];

                if (pathUrl.Contains('?'))
                    pathUrl = pathUrl.Split("?")[0];

                var url = baseUrl + pathUrl;
                result.Add(url);
            }
        }

        return result;
    }

    [GeneratedRegex(@"(?i)<a\s+[^>]*?href=[""']([^""']+)[""']\s+[^>]*?class=""chapter-link cp-l"">([^>]+)</a>", RegexOptions.None, "ru-RU")]
    private static partial Regex ChapterLinksRegex();

    [GeneratedRegex(@"(?i)readerInit.+(\[\[.+\]\])", RegexOptions.None, "ru-RU")]
    private static partial Regex ImagesFromChapterRegex();
}