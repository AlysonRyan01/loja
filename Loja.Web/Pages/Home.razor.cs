using Loja.Core.Handlers;
using Loja.Core.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages;

public partial class HomePage : ComponentBase
{
    #region properties

    public bool IsBusy { get; set; } = false;
    public List<Produto> Produtos { get; set; } = new();

    #endregion
    
    #region dependencies

    [Inject]public IProdutoHandler Handler { get; set; } = null!;
    [Inject]public IDialogService DialogService { get; set; } = null!;
    [Inject]public ISnackbar Snackbar { get; set; } = null!;
    
    #endregion
    
    #region overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var result = await Handler.ObterTodosProdutos();
            if (result.IsSuccess)
                Produtos = result.Dados ?? [];
            else
            {
                Snackbar.Add("Nenhum produto encontrado!", Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add($"Erro ao carregar a página:{e.Message}", Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
    

    #endregion
}