using System.Security.Claims;
using Dima.Web.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Layout;

public class MainLayoutPage : LayoutComponentBase
{
    #region properties
    
    public bool _openEnd = false;
    
    public bool _userLoggedIn = false;

    public string _userName { get; set; }

    public void ToggleEndDrawer()
    {
        _openEnd = !_openEnd;
    } 

    public bool IsBusy { get; set; } = false;
    public string SearchTerm { get; set; } = String.Empty;

    #endregion

    #region dependencies

    [Inject] public ICookieAuthenticationStateProvider AuthenticationState { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

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
                _userName = user.Identity.Name ?? String.Empty;
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