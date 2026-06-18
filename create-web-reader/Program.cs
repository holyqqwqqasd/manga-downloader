
using System.Text.Json;
using create_web_reader.Models;

var manga = JsonSerializer.Deserialize<MangaModel>(File.ReadAllText("..\\manga.json"));
var webManga = new MangaModel([]);

foreach (var ch in manga!.Chapters)
{
    var urlsWithIndex = ch.Images
        .Select((url, index) =>
        {
            var fileExtension = url.Split('.').Last();
            return $"{index}.{fileExtension}";
        })
        .ToList();
    var chapter = new ChapterModel(ch.Index, ch.Title, urlsWithIndex);
    webManga.Chapters.Add(chapter);
}

var options = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true
};
var json = JsonSerializer.Serialize(webManga, options);
File.WriteAllText(
    "..\\manga\\info.js",
    $"""
    var manga = {json};
    """);
