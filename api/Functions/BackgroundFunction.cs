using System.Net;
using Lmbtfy.Api.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Lmbtfy.Api.Functions;

public class BackgroundFunction
{
    private readonly BackgroundImageService _backgroundImageService;

    public BackgroundFunction(BackgroundImageService backgroundImageService)
    {
        _backgroundImageService = backgroundImageService;
    }

    [Function("Background")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "background")] HttpRequestData request,
        CancellationToken cancellationToken)
    {
        var background = await _backgroundImageService.GetDailyBackgroundAsync(cancellationToken);

        var response = request.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(background, cancellationToken);
        return response;
    }
}