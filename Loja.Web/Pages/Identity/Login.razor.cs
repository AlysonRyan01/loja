using Dima.Web.Security;
using Loja.Core.Handlers;
using Loja.Core.Requisicoes.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Identity;

public partial class LoginPage : ComponentBase
{
    #region Dependencies
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public ICookieAuthenticationStateProvider AuthenticationState { get; set; } = null!;
    [Inject] public IIdentityHandler Handler { get; set; } = null!;
    #endregion
    
    #region Properties
    public LoginRequest request { get; set; } = new();
    public bool IsBusy { get; set; } = false;
    #endregion
    
    #region Overrides
    protected override async Task OnInitializedAsync()
    {
        try
        {
            var result = await AuthenticationState.GetAuthenticationStateAsync();
            var user = result.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
                NavigationManager.NavigateTo("/");
            
        }
        catch
        {
            Snackbar.Add("Erro ao carregar a página", Severity.Error);
        }
    }
    #endregion

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        try
        {
            var result = await Handler.LoginAsync(request);

            if (result.IsSuccess)
            {
                Snackbar.Add($"{result.Mensagem}", Severity.Success);
                await Task.Delay(3000);
                NavigationManager.NavigateTo("/");
            }
            else
            {
                Snackbar.Add($"{result.Mensagem}", Severity.Error);
            }

        }
        catch (Exception e)
        {
            Snackbar.Add($"{e.Message}", Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
}