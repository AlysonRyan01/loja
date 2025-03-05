using System.Security.Claims;
using Loja.Core.Models;
using Loja.Core.Models.Identity;
using Loja.Core.Requisicoes.Endereco;
using Loja.Core.Requisicoes.Identity;
using Loja.Core.Respostas;

namespace Loja.Core.Handlers;

public interface IIdentityHandler 
{
    Task<Resposta<string>> LoginAsync(LoginRequest request);
    Task<Resposta<string>> RegisterAsync(RegisterRequest request);
    Task<Resposta<string>> LogoutAsync();
    Task<Resposta<UserInfo>> UserInfo(ClaimsPrincipal claimsPrincipal);
    Task<Resposta<IEnumerable<RoleClaim>>> UserRoles(ClaimsPrincipal logedUser);
    Task<Resposta<User>> UserInfoValidation(UserInfoValidationRequest request);
    Task<Resposta<Endereco>> UserAdressValidation(AtualizarEnderecoRequisicao request);
}