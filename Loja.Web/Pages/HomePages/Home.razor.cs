using System.Security.Claims;
using Dima.Web.Security;
using Dima.Web.Services;
using Loja.Core.Handlers;
using Loja.Core.Requisicoes.CarrinhoItens;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.HomePages;

public partial class HomePage : ComponentBase
{
    
    #region properties
    
    public bool IsBusy { get; set; } = false;
    public bool _userLoggedIn { get; set; } = false;
    public ClaimsPrincipal _user { get; set; }

    #endregion
    
    #region dependencies
    
    [Inject] public SearchService SearchService { get; set; }
    [Inject] public ICarrinhoItemHandler CarrinhoItemHandler { get; set; } = null!;
    [Inject] public ICookieAuthenticationStateProvider AuthenticationState { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    
    #endregion
    
    #region overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var result = await SearchService.CarregarProdutos();
            if (result.IsSuccess)
            {
                SearchService.FiltrarProdutos();
            }
            
        }
        catch (Exception e)
        {
            Snackbar.Add($"Erro ao carregar a página: {e.Message}", Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
        
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
    }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        
        SearchService.OnSearchChanged += AtualizarPagina;
        
    }
    

    #endregion

    public async Task AdicionarAoCarrinho(long produtoId)
    {
        try
        {
            var request = new CriarCarrinhoItemRequisicao
            {
                ProdutoId = produtoId,
                Quantidade = 1
            };

            if (_userLoggedIn == false)
            {
                Snackbar.Add("Você precisa fazer login para continuar", Severity.Info);
                NavigationManager.NavigateTo("/entrar");
                return;
            }
                
            var result = await CarrinhoItemHandler.CriarCarrinhoItemAsync(request, _user);
            if(result.IsSuccess)
                Snackbar.Add("Produto adicionado ao carrinho!", Severity.Success);
            else
            {
                Snackbar.Add(result.Mensagem ?? "Erro ao adicionar o produto ao carrinho", Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add("Erro ao adicionar o produto ao carrinho", Severity.Error);
        }
    }

    public void AtualizarPagina()
    {
        StateHasChanged();
    }

    public void Dispose()
    {
        SearchService.OnSearchChanged -= StateHasChanged;
    }
}
