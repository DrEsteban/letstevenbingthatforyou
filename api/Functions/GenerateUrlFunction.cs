using System.Net;
using System.Text.Json;
using Lmbtfy.Api.Models;
using Lmbtfy.Api.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Lmbtfy.Api.Functions;

public class GenerateUrlFunction
{
    private readonly ShareUrlService _shareUrlService;
    private readonly TinyUrlService _tinyUrlService;

    public GenerateUrlFunction(ShareUrlService shareUrlService, TinyUrlService tinyUrlService)
    {
        _shareUrlService = shareUrlService;
        _tinyUrlService = tinyUrlService;
    }

    [Function("GenerateUrl")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "generate-url")] HttpRequestData request,
        CancellationToken cancellationToken)
    {
        GenerateUrlRequest? payload;

        try
        {
            payload = await JsonSerializer.DeserializeAsync<GenerateUrlRequest>(request.Body, cancellationToken: cancellationToken);
        }
        catch (JsonException)
        {
            payload = null;
        }

        if (payload is null || string.IsNullOrWhiteSpace(payload.Query) || string.IsNullOrWhiteSpace(payload.Origin))
        {
            return await CreateBadRequestAsync(request, "Both query and origin are required.", cancellationToken);
        }

        string url;
        try
        {
            url = _shareUrlService.Build(payload.Origin, payload.Query);
        }
        catch (ArgumentException ex)
        {
            return await CreateBadRequestAsync(request, ex.Message, cancellationToken);
        }

        var tinyUrl = await _tinyUrlService.ShortenAsync(url, cancellationToken);
        var result = new GenerateUrlResponse(payload.Query.Trim(), url, tinyUrl);

        var response = request.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(result, cancellationToken);
        return response;
    }

    private static async Task<HttpResponseData> CreateBadRequestAsync(
        HttpRequestData request,
        string message,
        CancellationToken cancellationToken)
    {
        var response = request.CreateResponse(HttpStatusCode.BadRequest);
        await response.WriteAsJsonAsync(new { error = message }, cancellationToken);
        return response;
    }
}