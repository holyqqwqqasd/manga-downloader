using System.Collections.Concurrent;
using System.Text.Json;
using reader_urls.Helpers;
using reader_urls.Models;

var baseUrl = "https://3.readmanga.ru";
var mangaUrl = $"{baseUrl}/rycar__jivuchii_odnim_dnem";

Downloader.Init($"{baseUrl}/");

var (success, html) = await Downloader.ReadHtml(mangaUrl);
if (!success)
    return;

var chapters = Parser.ParseChapterLinks(html);

// Console.WriteLine($"Найдено глав: {chapters.Count}\n");
// foreach (var (text, link) in chapters)
// {
//     Console.WriteLine($"{text} = {baseUrl}{link}");
// }

var concurrentList = new ConcurrentBag<ChapterModel>();

var parallelOptions = new ParallelOptions
{
    MaxDegreeOfParallelism = 5 
};
await Parallel.ForEachAsync(chapters, parallelOptions, async (item, cancellationToken) =>
{
    var (success, htmlChapter) = await Downloader.ReadHtml($"{baseUrl}{item.Link}");

    Console.WriteLine($"{item.Title}: status " + (success ? "OK" : "FAILED!"));

    if (!success)
        return;

    var images = Parser.ParseImagesFromChapter(htmlChapter);

    concurrentList.Add(new ChapterModel(item.Index, item.Title, images));
});

var manga = new MangaModel([.. concurrentList.OrderBy(x => x.Index)]);

File.WriteAllText("..\\manga.json", JsonSerializer.Serialize(manga));
