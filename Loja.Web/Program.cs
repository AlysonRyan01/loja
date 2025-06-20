using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Dima.Web;
using Dima.Web.Handlers;
using Dima.Web.Security;
using Dima.Web.Services;
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
builder.Services.AddTransient<IEnderecoHandler, EnderecoHandler>();
builder.Services.AddTransient<IProdutoHandler, ProdutoHandler>();
builder.Services.AddTransient<ICarrinhoItemHandler, CarrinhoItemHandler>();
builder.Services.AddTransient<ICarrinhoHandler, CarrinhoHandler>();
builder.Services.AddTransient<IPedidoHandler, PedidoHandler>();
builder.Services.AddTransient<ICorreioHandler, CorreioHandler>();
builder.Services.AddTransient<IEmailHandler, EmailHandler>();
builder.Services.AddSingleton<LayoutService>();

builder.Services.AddSingleton<SearchService>();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.VisibleStateDuration = 1000;
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
