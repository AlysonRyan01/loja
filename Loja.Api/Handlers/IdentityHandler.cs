using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Security.Claims;
using Loja.Api.Data;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Models.Identity;
using Loja.Core.Requisicoes.Endereco;
using Loja.Core.Requisicoes.Identity;
using Loja.Core.Respostas;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Loja.Api.Handlers;

public class IdentityHandler : IIdentityHandler
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole<long>> _roleManager;
    private readonly LojaDataContext _context;
    private readonly IEnderecoHandler _enderecoHandler;
    
    public IdentityHandler(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole<long>> roleManager, LojaDataContext context, IEnderecoHandler enderecoHandler)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _roleManager = roleManager;
        _enderecoHandler = enderecoHandler;
    }
    
    public async Task<Resposta<string>> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return new Resposta<string>("Senha ou email incorretos", 401, "Senha ou email incorretos");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
                return new Resposta<string>("Senha ou email incorretos", 401, "Senha ou email incorretos");
            
            await _signInManager.SignInAsync(user, isPersistent: false);

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
            
            var roleExists = await _roleManager.RoleExistsAsync("User");
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole<long> { Name = "User", NormalizedName = "USER" });
            }
            
            var user = new User
            {
                FullName = request.FullName,
                UserName = request.FullName.Replace(" ", ""),
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            
            if (!result.Succeeded)
            {
                await transaction.RollbackAsync();
                return new Resposta<string>("Erro ao registrar usuário", 400, "Verifique os erros nos campos fornecidos.");
            }
            
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, request.Email));
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, request.FullName.Replace(" ", "")));
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            
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

            var endereco = new Endereco
            {
                UserId = user.Id
            };
            
            _context.Enderecos.Add(endereco);
            var enderecoResult = await _context.SaveChangesAsync();

            if (enderecoResult == 0)
            {
                await transaction.RollbackAsync();
                return new Resposta<string>("Erro ao criar o endereco", 400, "Erro ao associar endereco ao usuário.");
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

    public async Task<Resposta<UserInfo>> UserInfo(ClaimsPrincipal user)
    {
        try
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            var enderecoResult = await _enderecoHandler.ObterEnderecoPorUserId(new ObterEnderecoPorUserIdRequisicao
            {
                UserId = userId
            });
            
            var userInfos = await _userManager.FindByIdAsync(userId);
            
            if(userInfos == null || userInfos.Email == null)
                return new Resposta<UserInfo>(null, 404, "Usuario nao encontrado");

            var userInfo = new UserInfo
            {
                Id = userInfos.Id.ToString(),
                Name = userInfos.UserName,
                FullName = userInfos.FullName,
                Email = userInfos.Email,
                IsEmailConfirmed = userInfos.EmailConfirmed,
                Endereco = enderecoResult.Dados,
                PhoneNumber = userInfos.PhoneNumber
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
    
    public async Task<Resposta<User>> UserInfoValidation(UserInfoValidationRequest request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            
            if (user == null)
                return new Resposta<User>(null, 404, "Usuario nao encontrado");
            
            if (string.IsNullOrWhiteSpace(request.FullName))
                return new Resposta<User>(null, 400, "Nome completo não pode estar vazio");

            if (string.IsNullOrWhiteSpace(request.Email) || !new EmailAddressAttribute().IsValid(request.Email))
                return new Resposta<User>(null, 400, "E-mail inválido");
            
            user.Email = request.Email;
            user.UserName = request.FullName.Replace(" ", "");
            user.FullName = request.FullName;
            user.PhoneNumber = request.PhoneNumber;
            
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new Resposta<User>(null, 400, $"Falha ao atualizar usuário: {errors}");
            }
            
            await _context.SaveChangesAsync();

            return new Resposta<User>(user, 200, "Usuário validado e atualizado com sucesso!");
        }
        catch
        {
            return new Resposta<User>(null, 500, "Erro no servidor");
        }
    }

    public async Task<Resposta<Endereco>> UserAdressValidation(AtualizarEnderecoRequisicao request)
    {
        try
        {
            var endereco = await _context.Enderecos.FirstOrDefaultAsync(x => x.UserId.ToString() == request.UserId);

            if (endereco == null)
                return new Resposta<Endereco>(null, 404, "Endereco nao encontrado");

            endereco.Rua = request.Rua;
            endereco.Numero = request.Numero;
            endereco.Bairro = request.Bairro;
            endereco.Cidade = request.Cidade;
            endereco.Estado = request.Estado;
            endereco.CEP = request.CEP;

            _context.Enderecos.Update(endereco);
            await _context.SaveChangesAsync();

            return new Resposta<Endereco>(endereco, 200, "Usuário validado e atualizado com sucesso!");
        }
        catch
        {
            return new Resposta<Endereco>(null, 500, "Erro no servidor");
        }
    }
}