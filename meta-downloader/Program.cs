
using System.Text.Json;
using meta_downloader.Models;
using meta_downloader.Services;

var options = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true
};
var manga = JsonSerializer.Deserialize<MangaModel>(File.ReadAllText("..\\manga.json"), options);

foreach (var chapter in manga!.Chapters)
{
    var result = await ImageDownloader.DownloadImagesAsync(chapter.Images, $"..\\manga\\{chapter.Index}");

    if (!result)
    {
        Console.WriteLine($"Глава '{chapter.Title}' не была загружена!");
    }
}
