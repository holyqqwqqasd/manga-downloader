namespace reader_urls.Helpers;

public static class Downloader
{
    private static readonly HttpClient httpClient = new();

    public static void Init(string domain)
    {
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36");
        httpClient.DefaultRequestHeaders.Add("Referer", domain);
    }

    public static Task<string> ReadHtml(string url)
    {
        return httpClient.GetStringAsync(url);
    }
}