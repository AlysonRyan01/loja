using System.Security.Claims;
using Dima.Web.Security;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Models.Identity;
using Loja.Core.Requisicoes.Endereco;
using Loja.Core.Requisicoes.Produtos;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Exception = System.Exception;

namespace Dima.Web.Pages.CheckoutPages;

public partial class CheckoutPage : ComponentBase
{
    [Parameter] public string Slug { get; set; } = String.Empty;
    public bool _userLoggedIn { get; set; }
    public ClaimsPrincipal _user { get; set; }
    public Carrinho? carrinho { get; set; } 
    public Produto? produto { get; set; } = new Produto();
    public decimal Total { get; set; } 
    public bool IsBusy { get; set; } = false;
    public Endereco Endereco { get; set; } = new Endereco();
    public UserInfo UserInfo { get; set; } = new UserInfo();
    public CartaoDeCredito Cartao { get; set; } = new CartaoDeCredito();
    
    [Inject] public ICarrinhoHandler CarrinhoHandler { get; set; } = null!;
    [Inject] public IEnderecoHandler EnderecoHandler { get; set; } = null!;
    [Inject] public IProdutoHandler ProdutoHandler { get; set; } = null!;
    [Inject] public IIdentityHandler IdentityHandler { get; set; } = null!;
    [Inject] public ICookieAuthenticationStateProvider AuthenticationState { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    
    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var identityResult = await AuthenticationState.GetAuthenticationStateAsync();
            var user = identityResult.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                _userLoggedIn = true;
                _user = user;
                
                string userId = _user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                var userInfoResult = await IdentityHandler.UserInfo(user);

                if (userInfoResult.IsSuccess)
                {
                    UserInfo = userInfoResult.Dados ?? new UserInfo();
                    Endereco = userInfoResult?.Dados?.Endereco ?? new Endereco();
                }
            }
            else
            {
                _userLoggedIn = false;
            }

            StateHasChanged();

            if (Slug.StartsWith("carrinho-"))
            {
                if (_userLoggedIn)
                {
                    var carrinhoResult = await CarrinhoHandler.ObterCarrinhoPorUserAsync(_user);

                    if (carrinhoResult.IsSuccess)
                    {
                        carrinho = carrinhoResult.Dados;
                        Total = carrinho.ValorTotal;
                    }
                    else
                    {
                        Snackbar.Add("Não foi possível obter os produtos do carrinho", Severity.Error);
                    }
                }
                else
                {
                    NavigationManager.NavigateTo("/entrar");
                }
            }
            else if (Slug.StartsWith("produto-"))
            {
                string slugProduto = Slug.Replace("produto-", "");
                
                var result = await ProdutoHandler.ObterProdutoPorSlugAsync(new ObterProdutoPorSlugRequisicao
                {
                    Slug = slugProduto
                });

                if (result.IsSuccess)
                {
                    produto = result.Dados;
                    Total = produto.Preco;
                }
                else
                {
                    Snackbar.Add("Erro ao carregar o produto", Severity.Error);
                }
            }
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
}