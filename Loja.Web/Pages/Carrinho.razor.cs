using System.Security.Claims;
using Dima.Web.Security;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.CarrinhoItens;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages;

public partial class CarrinhoPage : ComponentBase
{
    #region properties

    public bool IsBusy { get; set; } = false;
    public bool _userLoggedIn { get; set; } = false;
    public List<CarrinhoItem> CarrinhoItens { get; set; } = new();
    public ClaimsPrincipal _user { get; set; }

    #endregion
    
    #region dependencies
    
    [Inject] public ICarrinhoItemHandler CarrinhoItemHandler { get; set; } = null!;
    [Inject] public ICookieAuthenticationStateProvider AuthenticationState { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    
    #endregion
    
    #region overrides

    protected override async Task OnInitializedAsync()
    {
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
        
        IsBusy = true;
        try
        {
            if (_userLoggedIn == false)
                return;
            else
            {
                var result = await CarrinhoItemHandler.ObterCarrinhoItemAsync(_user);
                if (result.IsSuccess)
                    CarrinhoItens = result.Dados ?? [];
                else
                {
                    Snackbar.Add("Nenhum produto encontrado!", Severity.Error);
                }
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