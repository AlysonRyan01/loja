using System.Security.Claims;
using Dima.Web.Security;
using Dima.Web.Services;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.CarrinhoItens;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.CarrinhoPages;

public partial class CarrinhoComponent : ComponentBase
{
    #region properties

    public bool IsBusy { get; set; } = false;
    public bool _userLoggedIn { get; set; } = false;
    public List<CarrinhoItem> CarrinhoItens { get; set; } = new();
    [Parameter] public string Slug { get; set; }
    public Carrinho Carrinho { get; set; } = new();
    public List<Produto> Produtos { get; set; } = new();
    public int QuantidadeProdutos { get; set; }
    public ClaimsPrincipal _user { get; set; }
    public string envio { get; set; }

    #endregion
    
    #region dependencies
    
    [Inject] public ICarrinhoItemHandler CarrinhoItemHandler { get; set; } = null!;
    [Inject] public ICarrinhoHandler CarrinhoHandler { get; set; } = null!;
    [Inject] public IProdutoHandler ProdutoHandler { get; set; } = null!;
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
            var produtoResult = await ProdutoHandler.ObterTodosProdutos();
            if(produtoResult.IsSuccess)
                Produtos = produtoResult.Dados ?? new();
            else
            {
                Snackbar.Add("Nenhum produto encontrado!", Severity.Error);
            }
            
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

            if (_userLoggedIn == false)
                return;

            var carrinhoItemResult = await CarrinhoItemHandler.ObterCarrinhoItemAsync(_user);
            if (carrinhoItemResult.IsSuccess)
                CarrinhoItens = carrinhoItemResult.Dados ?? [];
            else
            {
                Snackbar.Add("Nenhum produto encontrado!", Severity.Error);
            }

            var carrinhoResult = await CarrinhoHandler.ObterCarrinhoPorUserAsync(_user);
            if (carrinhoResult.IsSuccess)
            {
                if (carrinhoResult.Dados != null)
                    Carrinho = carrinhoResult.Dados;
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
        catch
        {
            Snackbar.Add("Erro na aplicação", Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion
    
    private async Task AtualizarCarrinhoItem()
    {
        try
        {
            var result = await CarrinhoItemHandler.ObterCarrinhoItemAsync(_user);
            if (result.IsSuccess)
            {
                CarrinhoItens = result.Dados ?? [];
            }
        }
        catch
        {
            Snackbar.Add("Erro ao atualizar os itens do carrinho", Severity.Error);
        }
    }
    
    private async Task AtualizarCarrinho()
    {
        try
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
        catch
        {
            Snackbar.Add("Erro ao atualizar o carrinho", Severity.Error);
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
        catch
        {
            Snackbar.Add("Erro ao remover o produto", Severity.Error);
        }
    }
}