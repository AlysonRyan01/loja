using System.Security.Claims;
using Dima.Web.Security;
using Dima.Web.Services;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.CarrinhoItens;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.LojaPages;

public partial class LojaPage : ComponentBase
{
    
    #region properties
    
    public bool IsBusy { get; set; } = false;
    public bool _userLoggedIn { get; set; } = false;
    public ClaimsPrincipal _user { get; set; }
    public bool _open = false;

    #endregion
    
    #region dependencies
    
    [Inject] public SearchService SearchService { get; set; }
    [Inject] public ICarrinhoItemHandler CarrinhoItemHandler { get; set; } = null!;
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
            var result = await SearchService.CarregarProdutos();
            if (result.IsSuccess)
            {
                SearchService.FiltrarProdutos();
            }

            foreach (var tv in SearchService.ProdutosFiltrados)
            {
                produtos.Add(tv);
                TVsFiltradas.Add(tv);
            }
            
        }
        catch (Exception e)
        {
            Snackbar.Add($"Erro ao carregar a página: {e.Message}", Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
        
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
    }
    
     
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        
        SearchService.OnSearchChanged += AtualizarPagina;
        
    }
    

    #endregion

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
            if (result.IsSuccess)
            {
                Snackbar.Add("Produto adicionado ao carrinho!", Severity.Success);
            }
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

    public void AtualizarPagina()
    {
        StateHasChanged();
    }

    public void Dispose()
    {
        SearchService.OnSearchChanged -= StateHasChanged;
    }
    
    private string busca;
    public string Busca
    {
        get => busca;
        set { busca = value; OnBuscarMudou(); }
    }

    
    public List<Produto> produtos = new List<Produto>();
    
    private bool filtroSamsung;
    public bool FiltroSamsung
    {
        get => filtroSamsung;
        set { filtroSamsung = value; OnFiltroChanged(); }
    }

    private bool filtroLG;
    public bool FiltroLG
    {
        get => filtroLG;
        set { filtroLG = value; OnFiltroChanged(); }
    }

    private bool filtroSony;
    public bool FiltroSony
    {
        get => filtroSony;
        set { filtroSony = value; OnFiltroChanged(); }
    }
    
    private bool filtroTCL;
    public bool FiltroTCL
    {
        get => filtroTCL;
        set { filtroTCL = value; OnFiltroChanged(); }
    }
    
    private bool filtroPhilco;
    public bool FiltroPhilco
    {
        get => filtroPhilco;
        set { filtroPhilco = value; OnFiltroChanged(); }
    }
    
    private bool filtroPhilips;
    public bool FiltroPhilips
    {
        get => filtroPhilips;
        set { filtroPhilips = value; OnFiltroChanged(); }
    }
    
    private bool filtroAOC;
    public bool FiltroAOC
    {
        get => filtroAOC;
        set { filtroAOC = value; OnFiltroChanged(); }
    }
    
    private bool filtroToshiba;
    public bool FiltroToshiba
    {
        get => filtroToshiba;
        set { filtroToshiba = value; OnFiltroChanged(); }
    }

    private decimal precoMax = 5000;
    public decimal PrecoMax
    {
        get => precoMax;
        set { precoMax = value; OnFiltroChanged(); }
    }

    public List<Produto> TVsFiltradas = new List<Produto>();
    
    public void OnFiltroChanged()
    {
        if (!FiltroSamsung && !FiltroSony && !FiltroLG && !FiltroTCL && !FiltroPhilco && !FiltroPhilips && !FiltroAOC && !FiltroToshiba)
        {
            TVsFiltradas = produtos.Where(tv => tv.Preco <= PrecoMax).ToList();
            return;
        }
        
        if (FiltroSamsung && FiltroSony && FiltroLG && FiltroTCL && FiltroPhilco && FiltroPhilips && FiltroAOC && FiltroToshiba)
        {
            TVsFiltradas = produtos.Where(tv => tv.Preco <= PrecoMax).ToList();
            return;
        }
        
        TVsFiltradas = produtos
            .Where(tv =>
                (FiltroSamsung && tv.Marca.Equals("Samsung", StringComparison.OrdinalIgnoreCase)) ||
                (FiltroLG && tv.Marca.Equals("LG", StringComparison.OrdinalIgnoreCase)) ||
                (FiltroSony && tv.Marca.Equals("Sony", StringComparison.OrdinalIgnoreCase)) ||
                (FiltroTCL && tv.Marca.Equals("TCL", StringComparison.OrdinalIgnoreCase)) ||
                (FiltroPhilco && tv.Marca.Equals("Philco", StringComparison.OrdinalIgnoreCase)) ||
                (FiltroPhilips && tv.Marca.Equals("Philips", StringComparison.OrdinalIgnoreCase)) ||
                (FiltroAOC && tv.Marca.Equals("Aoc", StringComparison.OrdinalIgnoreCase)) ||
                (FiltroToshiba && tv.Marca.Equals("Toshiba", StringComparison.OrdinalIgnoreCase))
            )
            .Where(tv => tv.Preco <= PrecoMax)
            .ToList();
        
    }
    
    public void OnBuscarMudou()
    {
        if (string.IsNullOrWhiteSpace(busca))
        {
            TVsFiltradas = produtos; 
        }
        else
        {
            TVsFiltradas = produtos
                .Where(p => p.Titulo.Contains(busca, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
    
    public void ToggleDrawer()
    {
        _open = !_open;
    }
    
}
