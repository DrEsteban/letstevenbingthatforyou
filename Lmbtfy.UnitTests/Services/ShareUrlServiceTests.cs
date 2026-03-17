using System;
using Lmbtfy.Api.Services;
using Xunit;

namespace Lmbtfy.UnitTests.Services;

public class ShareUrlServiceTests
{
    [Fact]
    public void Build_UsesSiteOriginAndEncodedQuery()
    {
        var service = new ShareUrlService();

        var result = service.Build("https://fancy-site.azurestaticapps.net", "find cute cats");

        Assert.Equal("https://fancy-site.azurestaticapps.net/?q=find%20cute%20cats", result);
    }

    [Fact]
    public void Build_RejectsNonHttpOrigins()
    {
        var service = new ShareUrlService();

        var error = Assert.Throws<ArgumentException>(() => service.Build("ftp://example.com", "query"));

        Assert.Contains("HTTP or HTTPS", error.Message);
    }
}