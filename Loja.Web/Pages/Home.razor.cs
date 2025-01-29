using Microsoft.AspNetCore.Components;

namespace Dima.Web.Pages;

public partial class HomePage : ComponentBase
{
    #region properties

    public bool IsBusy { get; set; } = false;
    public string SearchTerm { get; set; } = String.Empty;

    #endregion
}