using System.Security.Claims;
using Dima.Web.Security;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.Produtos;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Dima.Web.Pages;

public partial class ProdutoPageCode : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    [Parameter]public long id { get; set; }
    public Produto Produto { get; set; }
    public bool _userLoggedIn { get; set; } = false;
    public ClaimsPrincipal _user { get; set; }
    public string ImagemPrincipal { get; set; } = string.Empty;
    
    #endregion
    
    #region dependencies
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;
    [Inject] public IProdutoHandler handler { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public ICookieAuthenticationStateProvider AuthenticationState { get; set; }
    #endregion
    
    #region overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var result = await AuthenticationState.GetAuthenticationStateAsync();
            var user = result.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                _userLoggedIn = true;
                _user = user;
            }
            else
            {
                _userLoggedIn = false;
            }

            StateHasChanged();

        }
        catch
        {
            Snackbar.Add("Erro no authenticacao", Severity.Error);
        }

        try
        {
            var requisicao = new ObterProdutoPorIdRequisicao
            {
                Id = id
            };

            var result = await handler.ObterProdutoPorIdAsync(requisicao);
            if (result.IsSuccess)
            {
                Produto = result.Dados;
                ImagemPrincipal = $"{WebConfiguration.BackendUrl}{Produto.Imagens.First().Url}";
            }
            else
            {
                Snackbar.Add("Falha ao carregar as informações do produto...", Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add("Erro ao carregar produto...", Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion
    
    public async Task ChangeMainImage(string imageUrl)
    {
        await JSRuntime.InvokeVoidAsync("changeMainImage", imageUrl);
    }
    
}