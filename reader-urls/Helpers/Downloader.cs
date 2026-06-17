namespace reader_urls.Helpers;

public static class Downloader
{
    private static readonly HttpClient httpClient = new();

    public static void Init(string domain)
    {
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36");
        httpClient.DefaultRequestHeaders.Add("Referer", domain);
    }

    public static async Task<(bool, string)> ReadHtml(string url)
    {
        try
        {
            return (true, await httpClient.GetStringAsync(url));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Возникла ошибка при скачивании {url} ({ex.Message})");
            return (false, string.Empty);
        }
    }
}