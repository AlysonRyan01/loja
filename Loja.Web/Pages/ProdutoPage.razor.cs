using System.Security.Claims;
using Dima.Web.Security;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.CarrinhoItens;
using Loja.Core.Requisicoes.Produtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;

namespace Dima.Web.Pages;

public partial class ProdutoPageCode : ComponentBase
{
    #region Properties

    public bool _visible;
    public readonly DialogOptions _dialogOptions = new() { FullWidth = true };
    public bool exibirImagemTelaCheia = false;
    public ElementReference imgRef;
    public bool IsBusy { get; set; } = false;
    [Parameter]public long id { get; set; }
    public Produto Produto { get; set; }
    public List<Produto> Produtos { get; set; }
    public bool _userLoggedIn { get; set; } = false;
    public ClaimsPrincipal _user { get; set; }
    public string ImagemPrincipal { get; set; } = string.Empty;
    
    #endregion
    
    #region dependencies
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;
    [Inject] public IProdutoHandler handler { get; set; } = null!;
    [Inject] public ICarrinhoItemHandler CarrinhoItemHandler { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public ICookieAuthenticationStateProvider AuthenticationState { get; set; }
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
            var requisicao = new ObterProdutoPorIdRequisicao
            {
                Id = id
            };

            var result = await handler.ObterProdutoPorIdAsync(requisicao);
            if (result.IsSuccess)
            {
                Produto = result.Dados;
                ImagemPrincipal = $"{WebConfiguration.BackendUrl}{Produto.Imagens.First().Url}";
            }
            else
            {
                Snackbar.Add("Falha ao carregar as informações do produto...", Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add("Erro ao carregar produto...", Severity.Error);
        }

        try
        {
            var result = await handler.ObterTodosProdutos();
            if (result.IsSuccess)
            {
                Produtos = result.Dados ?? new List<Produto>();
            }
        }
        catch 
        {
            Snackbar.Add("Erro ao carregar os produtos recomendados...", Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    protected override async Task OnParametersSetAsync()
    {
        try
        {
            var requisicao = new ObterProdutoPorIdRequisicao
            {
                Id = id
            };

            var result = await handler.ObterProdutoPorIdAsync(requisicao);
            if (result.IsSuccess)
            {
                Produto = result.Dados;
                ImagemPrincipal = $"{WebConfiguration.BackendUrl}{Produto.Imagens.First().Url}";
            }
            else
            {
                Snackbar.Add("Falha ao carregar as informações do produto...", Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add("Erro ao carregar a pagina...", Severity.Error);
        }
    }

    #endregion
    
    public async Task ChangeMainImage(string imageUrl)
    {
        await JSRuntime.InvokeVoidAsync("changeMainImage", imageUrl);
    }
    
    public async Task AdicionarAoCarrinho(long produtoId)
    {
        try
        {
            var request = new CriarCarrinhoItemRequisicao
            {
                ProdutoId = produtoId,
                Quantidade = 1
            };

            if (_userLoggedIn == false)
            {
                Snackbar.Add("Você precisa fazer login para continuar", Severity.Info);
                NavigationManager.NavigateTo("/entrar");
                return;
            }
                
            var result = await CarrinhoItemHandler.CriarCarrinhoItemAsync(request, _user);
            if(result.IsSuccess)
                Snackbar.Add("Produto adicionado ao carrinho!", Severity.Success);
            else
            {
                Snackbar.Add(result.Mensagem ?? "Erro ao adicionar o produto ao carrinho", Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add("Erro ao adicionar o produto ao carrinho", Severity.Error);
        }
    }
    
    public async Task AplicarZoom(MouseEventArgs e)
    {
        await JSRuntime.InvokeVoidAsync("aplicarZoomImagem", imgRef, e);
    }
    
    public void MostrarImagemTelaCheia()
    {
        exibirImagemTelaCheia = true;
    }

    public void FecharImagemTelaCheia()
    {
        exibirImagemTelaCheia = false;
    }
    
    public void OpenDialog() => _visible = true;
    
    public void Submit() => _visible = false;
}