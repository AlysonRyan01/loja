using System.Net.Http.Json;
using System.Security.Claims;
using Loja.Core.Models.Identity;
using Loja.Core.Respostas;
using Microsoft.AspNetCore.Components.Authorization;

namespace Dima.Web.Security;

public class CookieAuthenticationStateProvider(IHttpClientFactory clientFactory) : AuthenticationStateProvider, ICookieAuthenticationStateProvider
{
    private bool isAuthenticated = false;
    private readonly HttpClient httpClient = clientFactory.CreateClient(WebConfiguration.HttpClientName);
    
    public async Task<bool> CheckAuthenticationAsync()
    {
        await GetAuthenticationStateAsync();
        return isAuthenticated;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        isAuthenticated = false;
        var user = new ClaimsPrincipal(new ClaimsIdentity());

        var userInfo = await GetUserAsync();
        if (userInfo == null)
            return new AuthenticationState(user);
        
        var claims = await GetClaimsAsync(userInfo);
        
        var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
        user = new ClaimsPrincipal(id);
        
        isAuthenticated = true;
        return new AuthenticationState(user);
    }

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private async Task<UserInfo?> GetUserAsync()
    {
        try
        {
            var user = await httpClient.GetFromJsonAsync<Resposta<UserInfo?>>("v1/identity/manage/info");
            return user.Dados;
        }
        catch
        {
            return null;
        }
    }

    private async Task<List<Claim>> GetClaimsAsync(UserInfo? user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
        };
        
        claims.AddRange(
            user.Claims.Where(x =>
                x.Key != ClaimTypes.Name &&
                x.Key != ClaimTypes.Email && 
                x.Key != ClaimTypes.NameIdentifier)
                .Select(x => new Claim(x.Key, x.Value)));

        RoleClaim[]? roles;

        try
        {
            roles = await httpClient.GetFromJsonAsync<RoleClaim[]>("v1/identity/roles");
        }
        catch
        {
            return claims;
        }

        foreach (var role in roles ?? [])
        {
            if(!string.IsNullOrEmpty(role.Type) && !string.IsNullOrEmpty(role.Value))
                claims.Add(new Claim(role.Type, role.Value, role.ValueType, role.Issuer, role.OriginalIssuer));
        }
            
        return claims;
    }
}