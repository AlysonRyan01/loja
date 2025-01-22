using System.Data.Common;
using System.Security.Claims;
using Loja.Api.Data;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Models.Identity;
using Loja.Core.Requisicoes.Identity;
using Loja.Core.Respostas;
using Microsoft.AspNetCore.Identity;
using User = Loja.Api.Models.User;

namespace Loja.Api.Handlers;

public class IdentityHandler : IIdentityHandler
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly LojaDataContext _context;
    
    public IdentityHandler(UserManager<User> userManager, SignInManager<User> signInManager, LojaDataContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }
    
    public async Task<Resposta<string>> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user == null)
                return new Resposta<string>("Usuario nao encontrado", 401, "Usuario nao encontrado");
            
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if(!result.Succeeded)
                return new Resposta<string>("Senha ou email incorretos", 401, "Senha ou email incorretos");
            
            return new Resposta<string>("Login realizado com sucesso!", 200, "Login realizado com sucesso");
        }
        catch (Exception e)
        {
            return new Resposta<string>(e.Message, 500, "Erro no login");
        }
    }

    public async Task<Resposta<string>> RegisterAsync(RegisterRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var carrinho = new Carrinho
            {
                CarrinhoItens = new List<CarrinhoItem>()
            };
            
            _context.Carrinhos.Add(carrinho);
            await _context.SaveChangesAsync();
            
            var user = new User
            {
                Email = request.Email,
                UserName = request.Email,
                CarrinhoId = carrinho.Id
            };
            
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                await transaction.RollbackAsync();
                return new Resposta<string>("Erro ao registrar usuário", 400, "Verifique os erros nos campos fornecidos.");
            }

            await transaction.CommitAsync();
            return new Resposta<string>("Usuário registrado com sucesso", 200, "Usuário registrado com sucesso");
        }
        catch (DbException e)
        {
            await transaction.RollbackAsync();
            return new Resposta<string>(e.Message, 500, "Erro no servidor");
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return new Resposta<string>(e.Message, 500, "Erro no servidor");
        }
    }

    public async Task<Resposta<string>> LogoutAsync()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return new Resposta<string>("Logout realizado com sucesso", 200, "Logout realizado com sucesso");
        }
        catch (Exception e)
        {
            return new Resposta<string>(e.Message, 500, "Erro no servidor");
        }
    }

    public async Task<Resposta<UserInfo>> UserInfo(ClaimsPrincipal logedUser)
    {
        try
        {
            var userId = logedUser?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if(string.IsNullOrEmpty(userId))
                return new Resposta<UserInfo>(null, 401, "Usuario nao autenticado");
            
            var user = await _userManager.FindByIdAsync(userId);
            
            if(user == null)
                return new Resposta<UserInfo>(null, 404, "Usuario nao encontrado");

            var userInfo = new UserInfo
            {
                Email = user.Email,
                IsEmailConfirmed = user.EmailConfirmed
            };
            
            return new Resposta<UserInfo>(userInfo, 200, "Usuario encontrado com sucesso");

        }
        catch (Exception e)
        {
            return new Resposta<UserInfo>(null, 500, "Erro no servidor");
        }
    }
    
    public async Task<Resposta<IEnumerable<RoleClaim>>> UserRoles(ClaimsPrincipal logedUser)
    {
        try
        {
            if(logedUser.Identity == null || !logedUser.Identity.IsAuthenticated)
                return new Resposta<IEnumerable<RoleClaim>>(null, 401, "Usuario nao autenticado");
            
            var identity = (ClaimsIdentity)logedUser.Identity;
            var roles = identity
                .FindAll(identity.RoleClaimType)
                .Select(x => new RoleClaim
                {
                    Issuer = x.Issuer,
                    OriginalIssuer = x.OriginalIssuer,
                    Type = x.Type,
                    Value = x.Value,
                    ValueType = x.ValueType
                });
            
            return new Resposta<IEnumerable<RoleClaim>>(roles, 200, "Roles encontradas com sucesso!");

        }
        catch (Exception e)
        {
            return new Resposta<IEnumerable<RoleClaim>>(null, 500, "Erro no servidor");
        }
    }
}