using System.Security.Claims;
using Dima.Web.Security;
using Dima.Web.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Layout;

public class MainLayoutPage : LayoutComponentBase
{
    
    #region properties
    
    public bool _open = false;
    
    public bool _userLoggedIn = false;

    public ClaimsPrincipal _user { get; set; }

    public void ToggleDrawer()
    {
        _open = !_open;
    } 

    public bool IsBusy { get; set; } = false;
    
    public string SearchTerm { get; set; } = string.Empty;

    #endregion

    #region dependencies

    [Inject] public ICookieAuthenticationStateProvider AuthenticationState { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public SearchService SearchService { get; set; } = null!;

    #endregion
    
    #region Overrides
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
    }
    #endregion
}