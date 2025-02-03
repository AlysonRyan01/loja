using System.Security.Claims;
using Dima.Web.Security;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.CarrinhoItens;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages;

public partial class CarrinhoComponent : ComponentBase
{
    #region properties

    public bool IsBusy { get; set; } = false;
    public bool _userLoggedIn { get; set; } = false;
    public List<CarrinhoItem> CarrinhoItens { get; set; } = new();
    public Carrinho Carrinho { get; set; } = new();
    public int QuantidadeProdutos { get; set; }
    public ClaimsPrincipal _user { get; set; }

    #endregion
    
    #region dependencies
    
    [Inject] public ICarrinhoItemHandler CarrinhoItemHandler { get; set; } = null!;
    [Inject] public ICarrinhoHandler CarrinhoHandler { get; set; } = null!;
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
        
        try
        {
            if (_userLoggedIn == false)
                return;
            
            var result = await CarrinhoItemHandler.ObterCarrinhoItemAsync(_user);
            if (result.IsSuccess)
                CarrinhoItens = result.Dados ?? [];
            else
            {
                Snackbar.Add("Nenhum produto encontrado!", Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add($"Erro ao carregar a página:{e.Message}", Severity.Error);
        }

        try
        {
            if (_userLoggedIn == false)
                return;
            
            var result = await CarrinhoHandler.ObterCarrinhoPorUserAsync(_user);
            if (result.IsSuccess)
            {
                if(result.Dados != null)
                    Carrinho = result.Dados;
                else
                {
                    Snackbar.Add("Erro ao carregar o carrinho", Severity.Error);
                }
            }
            else
            {
                Snackbar.Add("Nenhum carrinho encontrado!", Severity.Error);
            }

            foreach (var itens in Carrinho.CarrinhoItens)
            {
                QuantidadeProdutos += itens.Quantidade;
            }
            
            
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
    }
    #endregion
    
    private async Task AtualizarCarrinhoItem()
    {
        var result = await CarrinhoItemHandler.ObterCarrinhoItemAsync(_user);
        if (result.IsSuccess)
        {
            CarrinhoItens = result.Dados ?? [];
        }
    }
    
    private async Task AtualizarCarrinho()
    {
        QuantidadeProdutos = 0;
        var result = await CarrinhoHandler.ObterCarrinhoPorUserAsync(_user);
        if (result.IsSuccess)
        {
            if(result.Dados != null)
                Carrinho = result.Dados;
            
            foreach (var itens in Carrinho.CarrinhoItens)
            {
                QuantidadeProdutos += 1;
            }
        }
    }

    public async Task ExcluirDoCarrinho(long id)
    {
        var request = new RemoverCarrinhoItemRequisicao
        {
            Id = id
        };
        try
        {
            var result = await CarrinhoItemHandler.RemoverCarrinhoItemAsync(request, _user);
            if (result.IsSuccess)
            {
                Snackbar.Add("Produto removido com sucesso!", Severity.Success);
                await AtualizarCarrinhoItem();
                await AtualizarCarrinho();
                StateHasChanged();
            }
            else
            {
                Snackbar.Add("Erro ao remover o produto", Severity.Error);
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}