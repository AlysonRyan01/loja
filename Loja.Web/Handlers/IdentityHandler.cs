using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Loja.Core.Handlers;
using Loja.Core.Models.Identity;
using Loja.Core.Requisicoes.Identity;
using Loja.Core.Respostas;

namespace Dima.Web.Handlers;

public class IdentityHandler(IHttpClientFactory httpClientFactory) : IIdentityHandler
{
    private readonly HttpClient client = httpClientFactory.CreateClient(Configuration.HttpClientName);
    
    public async Task<Resposta<string>> LoginAsync(LoginRequest request)
    {
        var result = await client.PostAsJsonAsync("v1/identity/login", request);
        return result.IsSuccessStatusCode 
            ? new Resposta<string>("Login realizado com sucesso!", 200, "Login realizado com sucesso!")
            : new Resposta<string>(null, 400, "Não foi possível realizar o login");
    }

    public async Task<Resposta<string>> RegisterAsync(RegisterRequest request)
    {
        var result = await client.PostAsJsonAsync("v1/identity/register", request);
        return result.IsSuccessStatusCode 
            ? new Resposta<string>("Cadastro realizado com sucesso!", 201, "Cadastro realizado com sucesso!")
            : new Resposta<string>(null, 400, "Não foi possível realizar o cadastro");
    }

    public async Task<Resposta<string>> LogoutAsync()
    {
        var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
        var result = await client.PostAsJsonAsync("v1/identity/logout", emptyContent);
        return result.IsSuccessStatusCode 
            ? new Resposta<string>("Logout realizado com sucesso!", 201, "Logout realizado com sucesso!")
            : new Resposta<string>(null, 400, "Não foi possível realizar o logout");
    }

    public Task<Resposta<UserInfo>> UserInfo(ClaimsPrincipal logedUser)
    {
        throw new NotImplementedException();
    }

    public Task<Resposta<IEnumerable<RoleClaim>>> UserRoles(ClaimsPrincipal logedUser)
    {
        throw new NotImplementedException();
    }
}