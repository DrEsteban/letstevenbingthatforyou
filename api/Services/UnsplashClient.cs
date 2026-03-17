using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace Lmbtfy.Api.Services;

public class UnsplashClient
{
    private readonly HttpClient _httpClient;
    private readonly string? _accessKey;

    public UnsplashClient(IHttpClientFactory factory, IConfiguration configuration)
    {
        _httpClient = factory.CreateClient();
        _accessKey = configuration["UnsplashAccessKey"];
    }

    public async Task<string?> GetRandomPhotoAsync(string category, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_accessKey))
        {
            return null;
        }

        var endpoint =
            $"https://api.unsplash.com/photos/random?query={Uri.EscapeDataString(category)}&client_id={_accessKey}&count=1&orientation=landscape";

        using var response = await _httpClient.GetAsync(endpoint, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var results = await JsonSerializer.DeserializeAsync<List<UnsplashPhoto>>(contentStream, cancellationToken: cancellationToken);
        return results?.FirstOrDefault()?.Urls?.Regular;
    }

    private sealed class UnsplashPhoto
    {
        [JsonPropertyName("urls")]
        public UnsplashUrls? Urls { get; init; }
    }

    private sealed class UnsplashUrls
    {
        [JsonPropertyName("regular")]
        public string? Regular { get; init; }
    }
}