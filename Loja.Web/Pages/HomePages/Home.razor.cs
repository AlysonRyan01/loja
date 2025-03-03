using Dima.Web.Handlers;
using Loja.Core.Handlers;
using Loja.Core.Requisicoes.Contato;
using Loja.Core.Requisicoes.Email;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Dima.Web.Pages.HomePages;

public partial class HomePage : ComponentBase
{
    
    public ContatoRequisicao contato = new ContatoRequisicao();
    public bool valid;
    public bool IsBusy = false;
    public bool FormIsBusy = false;
    
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IEmailHandler EmailHandler { get; set; } = null!;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("initAOS");
            await JSRuntime.InvokeVoidAsync("initLeafletMap");
        }
    }
    
    public async Task EnviarFormulario()
    {
        FormIsBusy = true;
        try
        {
            if (string.IsNullOrEmpty(contato.Nome) || string.IsNullOrEmpty(contato.Email) ||
                string.IsNullOrEmpty(contato.Mensagem))
                Snackbar.Add("Preencha todos os campos", Severity.Error);
            else
            {
                var request = new SendEmailRequest
                {
                    Body = $"{contato.Mensagem} - {contato.Email} - {contato.Telefone}",
                    Subject = $"Enviado por {contato.Nome}",
                    ToEmail = "tvseletronica@tvseletronica.com.br"
                };

                var resultEmail = await EmailHandler.SendEmailAsync(request);

                if (resultEmail.IsSuccess)
                    Snackbar.Add("Mensagem enviada com sucesso! Aguarde o nosso Email", Severity.Success);
                else
                    Snackbar.Add("Ocorreu algum erro ao enviar a mensagem. Tente mais tarde..", Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add("Ocorreu algum erro ao enviar a mensagem. Tente mais tarde..", Severity.Error);
        }
        finally
        {
            FormIsBusy = false;
        }
    }
}