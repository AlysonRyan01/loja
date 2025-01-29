using Microsoft.AspNetCore.Components;

namespace Dima.Web.Layout;

public class MainLayoutPage : LayoutComponentBase
{
    #region properties

    public bool IsBusy { get; set; } = false;
    public string SearchTerm { get; set; } = String.Empty;

    #endregion
}