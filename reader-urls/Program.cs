using reader_urls.Helpers;

var baseUrl = "https://3.readmanga.ru";
var mangaUrl = $"{baseUrl}/rycar__jivuchii_odnim_dnem";

Downloader.Init($"{baseUrl}/");

var html = await Downloader.ReadHtml(mangaUrl);
var links = Parser.ParseChapterLinks(html);

// Вывод результата
Console.WriteLine($"Найдено ссылок: {links.Count}\n");
foreach (var link in links)
{
    Console.WriteLine($"{baseUrl}{link}");
}