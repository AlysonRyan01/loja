using Loja.Core.Handlers;
using Loja.Core.Requisicoes.Endereco;
using Loja.Core.Requisicoes.Identity;
using Loja.Core.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Api.Controllers;

[ApiController]
public class IdentityController(IIdentityHandler handler, ILogger<ProdutoController> logger)
    : ControllerBase
{
    [HttpPost("v1/identity/login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(new Resposta<string>("Erro de validacao", 401, "Erro de validacao"));
            
            var result = await handler.LoginAsync(request);
            
            return result.IsSuccess ? Ok(result) : Unauthorized(result);
                
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro no login", 401, e.Message));
        }
    }
    
    [HttpPost("v1/identity/register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(new Resposta<string>("Erro de validacao", 401, "Erro de validacao"));
            
            var result = await handler.RegisterAsync(request);
            
            return result.IsSuccess ? Ok(result) : Unauthorized(result);
                
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro no cadastro", 401, e.Message));
        }
    }

    [HttpPost("v1/identity/logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var result = await handler.LogoutAsync();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro no logout", 401, e.Message));
        }
    }

    [HttpGet("v1/identity/manage/info")]
    public async Task<IActionResult> GetUserInfo()
    {
        try
        {
            var user = HttpContext.User;
            
            var result = await handler.UserInfo(user);
            
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro ao obter o usuario.", 401, e.Message));
        }
    }
    
    [HttpGet("v1/identity/manage/roles")]
    public async Task<IActionResult> GetUserRoles()
    {
        try
        {
            var result = await handler.UserRoles(User);
            
            return result.IsSuccess ? Ok(result.Dados) : BadRequest(result.Dados);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro ao obter as roles.", 401, e.Message));
        }
    }

    [HttpPut("v1/identity/manage/update")]
    public async Task<IActionResult> UpdateUser(UserInfoValidationRequest request)
    {
        try
        {
            var result = await handler.UserInfoValidation(request);
            
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        catch (Exception e)
        {
            return BadRequest(new Resposta<string>("Erro ao atualizar o usuario.", 500, e.Message));
        }
    }
    
    [HttpPut("v1/identity/manage/update/adress")]
    public async Task<IActionResult> UpdateUserAdress(AtualizarEnderecoRequisicao request)
    {
        try
        {
            var result = await handler.UserAdressValidation(request);
            
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        catch (Exception e)
        {
            return BadRequest(new Resposta<string>("Erro ao atualizar o endereco.", 500, e.Message));
        }
    }
    
}