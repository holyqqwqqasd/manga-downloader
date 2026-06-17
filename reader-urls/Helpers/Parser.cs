using System.Text.RegularExpressions;

namespace reader_urls.Helpers;

public static class Parser
{
    public static List<string> ParseChapterLinks(string html)
    {
        // Регулярное выражение для извлечения ссылок
        // (?i)          : игнорировать регистр (A-Z = a-z)
        // <a\s+         : тег <a и хотя бы один пробел
        // [^>]*?        : любые символы, кроме '>', не жадно (чтобы не перескочить на другой тег)
        // href=['""]    : атрибут href=" или href='
        // (['""]?)      : (необязательно) захват типа кавычки, чтобы закрыть её такой же
        // ([^'""]+)     : ГРУППА 1: захватываем саму ссылку (всё, что не является кавычкой)
        // ['""]         : закрывающая кавычка
        const string pattern = @"(?i)<a\s+[^>]*?href=[""']([^""']+)[""']\s+[^>]*?class=""chapter-link cp-l""";

        var links = new List<string>();

        // Находим все совпадения
        var matches = Regex.Matches(html, pattern);

        foreach (Match match in matches)
        {
            // Groups[1] содержит значение, попавшее в первые скобки ([^""']+)
            var link = match.Groups[1].Value;
            links.Add(link);
        }

        return links;
    }
}