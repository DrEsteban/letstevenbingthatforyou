namespace Lmbtfy.Api.Services;

public class ShareUrlService
{
    public string Build(string origin, string query)
    {
        if (!Uri.TryCreate(origin, UriKind.Absolute, out var baseUri))
        {
            throw new ArgumentException("Origin must be an absolute URI.", nameof(origin));
        }

        if (!string.Equals(baseUri.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(baseUri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Origin must use HTTP or HTTPS.", nameof(origin));
        }

        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException("Query is required.", nameof(query));
        }

        var normalizedOrigin = baseUri.GetLeftPart(UriPartial.Authority).TrimEnd('/');
        return $"{normalizedOrigin}/?q={Uri.EscapeDataString(query.Trim())}";
    }
}