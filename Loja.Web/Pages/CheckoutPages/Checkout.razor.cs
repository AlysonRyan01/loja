using System.Globalization;
using System.Security.Claims;
using Dima.Web.Handlers;
using Dima.Web.Security;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Models.Identity;
using Loja.Core.Requisicoes.Correios;
using Loja.Core.Requisicoes.Endereco;
using Loja.Core.Requisicoes.Identity;
using Loja.Core.Requisicoes.Produtos;
using Loja.Core.Respostas.MelhorEnvio;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Exception = System.Exception;

namespace Dima.Web.Pages.CheckoutPages;

public partial class CheckoutPage : ComponentBase
{
    public MudForm UserInfoForm;
    public MudForm UserAdressForm;
    public bool UserInfoIsValid { get; set; } = false;
    public bool UserAddressIsValid { get; set; } = false;
    public bool FreteIsValid { get; set; } = false;
    [Parameter] public string Slug { get; set; } = String.Empty;
    public bool _userLoggedIn { get; set; }
    public ClaimsPrincipal _user { get; set; }
    public Carrinho? carrinho { get; set; } 
    public Produto? produto { get; set; } = new Produto();
    public decimal TotalCarrinho { get; set; }
    public decimal TotalProduto { get; set; }
    public bool IsBusy { get; set; } = false;
    public Endereco Endereco { get; set; } = new Endereco();
    public UserInfo UserInfo { get; set; } = new UserInfo();
    public CartaoDeCredito Cartao { get; set; } = new CartaoDeCredito();
    public ElementReference EnderecoSection;
    public ElementReference FreteSection;
    public ElementReference PagamentoSection;
    public List<CalculoFreteResponse> Fretes { get; set; }
    public CalculoFreteResponse FreteSelecionado;
    public AtualizarEnderecoRequisicao endereco { get; set; } = new();
    public PatternMask CepMask = new PatternMask("00000-000")
    {
        MaskChars = new[] { new MaskChar('0', @"[0-9]") }
    };
    
    [Inject] public ICarrinhoHandler CarrinhoHandler { get; set; } = null!;
    [Inject] public IEnderecoHandler EnderecoHandler { get; set; } = null!;
    [Inject] public IProdutoHandler ProdutoHandler { get; set; } = null!;
    [Inject] public IIdentityHandler IdentityHandler { get; set; } = null!;
    [Inject] public ICookieAuthenticationStateProvider AuthenticationState { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;
    [Inject] public ICorreioHandler CorreioHandler { get; set; } = null!;
    
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
                        TotalCarrinho = carrinho.ValorTotal;
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

    public async Task UserInfoValidation()
    {
        try
        {
            await UserInfoForm.Validate();

            if (!UserInfoForm.IsValid)
            {
                Console.WriteLine("Preencha o formulario corretamente");
                return;
            }
            
            var request = new UserInfoValidationRequest
            {
                UserId = UserInfo.Id,
                Email = UserInfo.Email,
                FullName = UserInfo.FullName,
                PhoneNumber = UserInfo.PhoneNumber
            };
            
            var result = await IdentityHandler.UserInfoValidation(request);

            if (result.IsSuccess)
            {
                UserInfoIsValid = true;
                StateHasChanged();
                Snackbar.Add("Dados validados com sucesso!", Severity.Success);
                await ScrollToEndereco();
            }
            else
            {
                Snackbar.Add("Dados invalidos!", Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
    }
    
    public async Task UserAdressValidation()
    {
        try
        {
            await UserAdressForm.Validate();

            if (!UserAdressForm.IsValid)
            {
                Console.WriteLine("Preencha o formulario corretamente");
                return;
            }
            
            endereco = new AtualizarEnderecoRequisicao
            {
                UserId = UserInfo.Id,
                Rua = Endereco.Rua,
                Numero = Endereco.Numero,
                Bairro = Endereco.Bairro,
                Cidade = Endereco.Cidade,
                Estado = Endereco.Estado,
                CEP = Endereco.CEP.Replace("-", "") ?? string.Empty,
                Pais = "Brasil"
            };
            
            var result = await IdentityHandler.UserAdressValidation(endereco);

            if (result.IsSuccess)
            {
                UserAddressIsValid = true;
                
                var request = new CalcularFreteRequest
                {
                    CepDestino = endereco.CEP,
                    Comprimento = produto.Largura.ToString(),
                    Altura = produto.Altura.ToString(),
                };
                
                var freteResult = await CorreioHandler.CalcularFreteAsync(request);

                if (freteResult.IsSuccess)
                {
                    Fretes = freteResult.Dados;
                }
                
                StateHasChanged();
                Snackbar.Add("Endereço validado com sucesso!", Severity.Success);
                await ScrollToFrete();
            }
            else
            {
                Snackbar.Add("Dados invalidos!", Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
    }
    
    public async Task FreteValidation()
    {
        try
        {
            if (FreteSelecionado == null)
            {
                Snackbar.Add("Nenhum frete selecionado. Por favor, selecione um frete.", Severity.Error);
                return;
            }
            
            if (!string.IsNullOrEmpty(FreteSelecionado.Name) && !string.IsNullOrEmpty(FreteSelecionado.Price))
            {
                FreteIsValid = true;
                
                decimal valorFrete = decimal.Parse(FreteSelecionado.Price, CultureInfo.InvariantCulture);
                TotalProduto = produto.Preco + valorFrete;
                
                Snackbar.Add("Frete validado com sucesso!", Severity.Success);
                
                StateHasChanged();
                await Task.Delay(10);
                
                await ScrollToPagamento();
            }
            else
            {
                Snackbar.Add("Falha ao validar o frete, tente novamente", Severity.Error);
            }


        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
    }
    
    public string ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return "O telefone é obrigatório";
        
        if (phone.Length != 11)
            return "O telefone precisa ter 11 números";
        
        return null;
    }
    
    public string ValidateCEP(string cep)
    {
        if (string.IsNullOrWhiteSpace(cep))
            return "O campo CEP é obrigatório";
        
        string cepSemHifen = cep.Replace("-", "");
        
        if (cepSemHifen.Length != 8)
            return "O CEP deve conter 8 caracteres.";
        
        return null;
    }

    public void FormatPhoneNumber()
    {
        if (!string.IsNullOrWhiteSpace(UserInfo.PhoneNumber))
        {
            var cleanPhone = new string(UserInfo.PhoneNumber.Where(char.IsDigit).ToArray());
            
            if (cleanPhone.Length == 11)
            {
                UserInfo.PhoneNumber = string.Format("({0}) {1}-{2}",
                    cleanPhone.Substring(0, 2),
                    cleanPhone.Substring(2, 5),
                    cleanPhone.Substring(7, 4));
            }
        }
    }
    
    private async Task ScrollToEndereco()
    {
        await JSRuntime.InvokeVoidAsync("scrollIntoView", EnderecoSection);
    }
    
    private async Task ScrollToFrete()
    {
        await JSRuntime.InvokeVoidAsync("scrollIntoView", FreteSection);
    }
    
    private async Task ScrollToPagamento()
    {
        await JSRuntime.InvokeVoidAsync("scrollIntoView", PagamentoSection);
    }

}