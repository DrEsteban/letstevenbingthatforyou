using Blazored.Modal;
using Lmbtfy.Blazor;
using Lmbtfy.Blazor.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient("Zip1", client =>
    client.BaseAddress = new Uri("https://zip1.io/"));

builder.Services.AddBlazoredModal();

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IImageService, ImageService>();

await builder.Build().RunAsync();