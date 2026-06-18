
using System.Text.Json;
using meta_downloader.Models;
using meta_downloader.Services;

var json = File.ReadAllText("..\\manga.json");
var manga = JsonSerializer.Deserialize<MangaModel>(json);

foreach (var chapter in manga!.Chapters)
{
    var result = await ImageDownloader.DownloadImagesAsync(chapter.Images, $"..\\manga\\{chapter.Title}");

    if (!result)
    {
        Console.WriteLine($"Глава '{chapter.Title}' не была загружена!");
    }
}
