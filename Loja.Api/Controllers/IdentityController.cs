using Loja.Core.Handlers;
using Loja.Core.Requisicoes.Identity;
using Loja.Core.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Api.Controllers;

[Route("v1/identity/")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IIdentityHandler _handler;
    private readonly ILogger<ProdutoController> _logger;

    public IdentityController(IIdentityHandler handler, ILogger<ProdutoController> logger)
    {
        _handler = handler;
        _logger = logger;
    }
    
    [HttpPost("/login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(new Resposta<string>("Erro no ModelState", 401, "Erro no ModelState"));
            
            var result = await _handler.LoginAsync(request);
            
            return result.IsSuccess ? Ok(result.Mensagem) : Unauthorized(result.Mensagem);
                
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro no login", 401, e.Message));
        }
    }
    
    [HttpPost("/register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(new Resposta<string>("Erro no ModelState", 401, "Erro no ModelState"));
            
            var result = await _handler.RegisterAsync(request);
            
            return result.IsSuccess ? Ok(result.Mensagem) : Unauthorized(result.Mensagem);
                
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro no cadastro", 401, e.Message));
        }
    }

    [HttpPost("/logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var result = await _handler.LogoutAsync();
            return result.IsSuccess ? Ok(result.Mensagem) : BadRequest(result.Mensagem);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro no logout", 401, e.Message));
        }
    }

    [HttpGet("/manage/info")]
    public async Task<IActionResult> GetUserInfo()
    {
        try
        {
            var result = await _handler.UserInfo(User);
            
            return result.IsSuccess ? Ok(result.Dados) : BadRequest(result.Mensagem);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro ao obter o usuario.", 401, e.Message));
        }
    }
    
    [HttpGet("/manage/roles")]
    public async Task<IActionResult> GetUserRoles()
    {
        try
        {
            var result = await _handler.UserRoles(User);
            
            return result.IsSuccess ? Ok(result.Dados) : BadRequest(result.Mensagem);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro ao obter as roles.", 401, e.Message));
        }
    }
}