using System.Security.Claims;
using System.Security.Principal;
using Loja.Core.Models.Identity;
using Loja.Core.Requisicoes.Identity;
using Loja.Core.Respostas;

namespace Loja.Core.Handlers;

public interface IIdentityHandler 
{
    Task<Resposta<string>> LoginAsync(LoginRequest Request);
    Task<Resposta<string>> RegisterAsync(RegisterRequest Request);
    Task<Resposta<string>> LogoutAsync();
    Task<Resposta<UserInfo>> UserInfo(ClaimsPrincipal logedUser);
    Task<Resposta<IEnumerable<RoleClaim>>> UserRoles(ClaimsPrincipal logedUser);
}