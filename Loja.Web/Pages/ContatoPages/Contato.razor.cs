using Loja.Core.Requisicoes.Contato;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Dima.Web.Pages.ContatoPages;

public partial class ContatoPage : ComponentBase
{
    public ContatoRequisicao contato { get; set; } = new();
    public bool valid;
    public bool IsBusy = false;
    
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("initLeafletMap");
        }
    }
    
    public void EnviarFormulario()
    {
        Console.WriteLine($"Nome: {contato.Nome}, E-mail: {contato.Email}, Mensagem: {contato.Mensagem}");
        Snackbar.Add("Mensagem enviada com sucesso!", Severity.Success);
    }
}