namespace meta_downloader.Services;

public class ImageDownloader
{
    // Один экземпляр HttpClient на все время работы приложения
    private static readonly HttpClient _httpClient = new();

    public static async Task<bool> DownloadImagesAsync(List<string> urls, string outputFolder)
    {
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        var urlsWithIndex = urls.Select((x, i) => (x, i)).ToList();

        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = 10
        };

        var result = true;

        await Parallel.ForEachAsync(urlsWithIndex, parallelOptions, async (x, cancellationToken) =>
        {
            var (url, index) = x;

            try
            {
                var fileExtension = url.Split('.').Last();
                var filePath = Path.Combine(outputFolder, $"{index}.{fileExtension}");

                if (File.Exists(filePath))
                {
                    Console.WriteLine($"SKIP {url}");
                    return;
                }

                using var response = await _httpClient.GetAsync(url, cancellationToken);

                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);

                await stream.CopyToAsync(fileStream, cancellationToken);

                Console.WriteLine($"OK {url}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки {url}: {ex.Message}");

                Interlocked.CompareExchange(ref result, false, true);
            }
        });

        return result;
    }
}
