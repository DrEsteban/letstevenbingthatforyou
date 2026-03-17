using Lmbtfy.Api.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Lmbtfy.Api.Services;

public class BackgroundImageService
{
    private readonly IMemoryCache _memoryCache;
    private readonly DailyKeywordService _dailyKeywordService;
    private readonly UnsplashClient _unsplashClient;

    public BackgroundImageService(
        IMemoryCache memoryCache,
        DailyKeywordService dailyKeywordService,
        UnsplashClient unsplashClient)
    {
        _memoryCache = memoryCache;
        _dailyKeywordService = dailyKeywordService;
        _unsplashClient = unsplashClient;
    }

    public async Task<BackgroundResponse> GetDailyBackgroundAsync(CancellationToken cancellationToken = default)
    {
        var category = _dailyKeywordService.GetDailyKeyword();
        var cacheKey = $"background:{DateTime.UtcNow:yyyyMMdd}:{category}";

        var imageUrl = await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
            return await _unsplashClient.GetRandomPhotoAsync(category, cancellationToken);
        });

        return new BackgroundResponse(category, imageUrl);
    }
}