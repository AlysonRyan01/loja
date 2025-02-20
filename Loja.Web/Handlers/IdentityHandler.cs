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
    private readonly HttpClient client = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);
    
    public async Task<Resposta<string>> LoginAsync(LoginRequest request)
    {
        var result = await client.PostAsJsonAsync("v1/identity/login", request);
        return result.IsSuccessStatusCode 
            ? new Resposta<string>("Login realizado com sucesso!", 200, "Login realizado com sucesso!")
            : new Resposta<string>(null, 400, "E-mail ou senha incorretos!");
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

    public async Task<Resposta<UserInfo>> UserInfo(ClaimsPrincipal user)
        => await client.GetFromJsonAsync<Resposta<UserInfo>>($"v1/identity/manage/info")
           ?? new Resposta<UserInfo>(null, 400, "Não foi possível recuperar os dados do usuario");

    public Task<Resposta<IEnumerable<RoleClaim>>> UserRoles(ClaimsPrincipal logedUser)
    {
        var roles = logedUser.FindAll(ClaimTypes.Role).Select(x => new RoleClaim { Value = x.Value });
        return Task.FromResult(new Resposta<IEnumerable<RoleClaim>>(roles, 200, "Roles encontradas com sucesso!"));
    }

    public async Task<Resposta<IEnumerable<RoleClaim>>> UserClaims(ClaimsPrincipal logedUser)
    {
        var response = await client.GetFromJsonAsync<IEnumerable<RoleClaim>>("v1/identity/manage/claims");
    
        if (response != null)
        {
            return new Resposta<IEnumerable<RoleClaim>>(response, 200, "Claims recuperadas com sucesso.");
        }
    
        return new Resposta<IEnumerable<RoleClaim>>(null, 400, "Erro ao recuperar as claims.");
    }
}