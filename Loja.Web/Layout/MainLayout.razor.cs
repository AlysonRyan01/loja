using System.Security.Claims;
using Dima.Web.Security;
using Dima.Web.Services;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Dima.Web.Layout;

public class MainLayoutPage : LayoutComponentBase
{
    
    #region properties
    
    public bool _open = false;
    
    public bool _userLoggedIn = false;

    public ClaimsPrincipal _user { get; set; }

    public string Username { get; set; } = "null";

    public void ToggleDrawer()
    {
        _open = !_open;
    }

    public bool IsBusy { get; set; } = false;
    
    public string SearchTerm { get; set; }

    public Carrinho carrinho { get; set; } = new Carrinho();
    
    public MudMenu menu;

    #endregion

    #region dependencies

    [Inject] public ICookieAuthenticationStateProvider AuthenticationState { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public SearchService SearchService { get; set; } = null!;
    [Inject] public ICarrinhoHandler CarrinhoHandler { get; set; } = null!;
    [Inject] public IIdentityHandler IdentityHandler { get; set; } = null!;
    [Inject] public LayoutService LayoutService { get; set; } = null!;

    #endregion
    
    #region Overrides
    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            LayoutService.OnChange += AtualizarLayout;
            var result = await AuthenticationState.GetAuthenticationStateAsync();
            var user = result.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                _userLoggedIn = true;
                _user = user;
                
                var identityResult = await IdentityHandler.UserInfo(user);
                if (identityResult.IsSuccess)
                {
                    Username = identityResult.Dados.FullName;
                }
                
                var carrinhoResult = await CarrinhoHandler.ObterCarrinhoPorUserAsync(user);
                if (carrinhoResult.IsSuccess)
                    carrinho = carrinhoResult.Dados ?? new ();

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
        finally
        {
            IsBusy = false;
        }
    }
    #endregion
    
    public void HandleKeyPress(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            SearchService.SearchTerm = SearchTerm;
        }
    }

    public void HandleMouseClick()
    {
        SearchService.SearchTerm = SearchTerm;
    }

    public void LimparCampoPesquisa()
    {
        SearchTerm = string.Empty;
        SearchService.SearchTerm = SearchTerm;
    }
    
    private void AtualizarLayout()
    {
        StateHasChanged();
    }

    public async ValueTask DisposeAsync()
    {
        LayoutService.OnChange -= AtualizarLayout;
    }
}