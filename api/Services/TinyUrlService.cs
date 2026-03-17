namespace Lmbtfy.Api.Services;

public class TinyUrlService
{
    private readonly HttpClient _httpClient;

    public TinyUrlService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    public async Task<string> ShortenAsync(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _httpClient.GetStringAsync(
                $"https://tinyurl.com/api-create.php?url={Uri.EscapeDataString(url)}",
                cancellationToken);
        }
        catch
        {
            return url;
        }
    }
}