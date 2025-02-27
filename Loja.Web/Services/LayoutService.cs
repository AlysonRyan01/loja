namespace Dima.Web.Services;

public class LayoutService
{
    public event Action OnChange;

    public void NotifyStateChanged() => OnChange?.Invoke();
}