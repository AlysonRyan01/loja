using System.Data.Common;
using System.Security.Claims;
using Loja.Api.Data;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Models.Identity;
using Loja.Core.Requisicoes.Identity;
using Loja.Core.Respostas;
using Microsoft.AspNetCore.Identity;


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
                return new Resposta<string>("Senha ou email incorretos", 401, "Senha ou email incorretos");
            
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if(!result.Succeeded)
                return new Resposta<string>("Senha ou email incorretos", 401, "Senha ou email incorretos");
            
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            
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
            var user = new User
            {
                Email = request.Email,
                UserName = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            
            if (!result.Succeeded)
            {
                await transaction.RollbackAsync();
                return new Resposta<string>("Erro ao registrar usuário", 400, "Verifique os erros nos campos fornecidos.");
            }
            
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, request.Email));
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, request.Email));
            await _userManager.AddToRoleAsync(user, "User");

            var carrinho = new Carrinho
            {
                UserId = user.Id,
            };
            
            _context.Carrinhos.Add(carrinho); 
            var carrinhoResult = await _context.SaveChangesAsync();

            if (carrinhoResult == 0)
            {
                await transaction.RollbackAsync();
                return new Resposta<string>("Erro ao criar carrinho", 400, "Erro ao associar carrinho ao usuário.");
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
            var userId = logedUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if(string.IsNullOrEmpty(userId))
                return new Resposta<UserInfo>(null, 401, "Usuario nao autenticado");
            
            var user = await _userManager.FindByIdAsync(userId);
            
            if(user == null || user.Email == null)
                return new Resposta<UserInfo>(null, 404, "Usuario nao encontrado");

            var userInfo = new UserInfo
            {
                Email = user.Email,
                IsEmailConfirmed = user.EmailConfirmed
            };
            
            return new Resposta<UserInfo>(userInfo, 200, "Usuario encontrado com sucesso");

        }
        catch
        {
            return new Resposta<UserInfo>(null, 500, "Erro no servidor");
        }
    }
    
    public Task<Resposta<IEnumerable<RoleClaim>>> UserRoles(ClaimsPrincipal logedUser)
    {
        try
        {
            if(logedUser.Identity == null || !logedUser.Identity.IsAuthenticated)
                return Task.FromResult(new Resposta<IEnumerable<RoleClaim>>(null, 401, "Usuario nao autenticado"));
            
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
            
            return Task.FromResult(new Resposta<IEnumerable<RoleClaim>>(roles, 200, "Roles encontradas com sucesso!"));

        }
        catch
        {
            return Task.FromResult(new Resposta<IEnumerable<RoleClaim>>(null, 500, "Erro no servidor"));
        }
    }
}