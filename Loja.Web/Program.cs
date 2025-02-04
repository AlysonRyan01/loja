using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Dima.Web;
using Dima.Web.Handlers;
using Dima.Web.Security;
using Loja.Core.Handlers;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

WebConfiguration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? String.Empty;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CookieHandler>();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
builder.Services.AddScoped(x => (ICookieAuthenticationStateProvider)x.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddTransient<IIdentityHandler, IdentityHandler>();
builder.Services.AddTransient<IProdutoHandler, ProdutoHandler>();
builder.Services.AddTransient<ICarrinhoItemHandler, CarrinhoItemHandler>();
builder.Services.AddTransient<ICarrinhoHandler, CarrinhoHandler>();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.VisibleStateDuration = 1000; // Tempo em milissegundos (2 segundos)
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.MaxDisplayedSnackbars = 3;
});

builder.Services.AddHttpClient(WebConfiguration.HttpClientName,
    client =>
    {
        client.BaseAddress = new Uri(WebConfiguration.BackendUrl);
    }).AddHttpMessageHandler<CookieHandler>();

await builder.Build().RunAsync();
