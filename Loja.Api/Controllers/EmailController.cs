using Loja.Api.Services;
using Loja.Core.Requisicoes.Email;
using Loja.Core.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Api.Controllers;

[ApiController]
public class EmailController : ControllerBase
{
    [HttpPost("v1/email/send")]
    public async Task<IActionResult> SendEmailAsync([FromServices] EmailService emailService, SendEmailRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<string>("Erro de validação", 400, "Erro de validação"));
            
            var result = emailService.SendEmail(request);

            if (result == false)
                return BadRequest(new Resposta<string>("Não foi possivel enviar o email", 400, "Não foi possivel enviar o email"));
                
            return Ok(new Resposta<string>("Email enviado com sucesso!", 200, "Email enviado com sucesso!"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}