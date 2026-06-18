
using System.Text.Json;
using meta_downloader.Models;
using meta_downloader.Services;

var manga = JsonSerializer.Deserialize<MangaModel>(File.ReadAllText("..\\manga.json"));

foreach (var chapter in manga!.Chapters)
{
    var result = await ImageDownloader.DownloadImagesAsync(chapter.Images, $"..\\manga\\{chapter.Index}");

    if (!result)
    {
        Console.WriteLine($"Глава '{chapter.Title}' не была загружена!");
    }
}
