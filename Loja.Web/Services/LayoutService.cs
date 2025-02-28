namespace Dima.Web.Services;

public class LayoutService
{
    public event Func<Task>? OnCarrinhoAtualizado;

    public async Task NotifyCarrinhoAtualizadoAsync()
    {
        if (OnCarrinhoAtualizado is not null)
        {
            await OnCarrinhoAtualizado.Invoke();
        }
    }
}
