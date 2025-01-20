using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Dima.Web;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient(Configuration.HttpClientName,
    client =>
    {
        client.BaseAddress = new Uri(Configuration.BackendUrl);
    });
builder.Services.AddMudServices();

await builder.Build().RunAsync();
