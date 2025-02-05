using Dima.Web.Handlers;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Respostas;

namespace Dima.Web.Services;

public class SearchService
{
    private string _searchTerm;
    public event Action OnSearchChanged;
    private readonly IProdutoHandler ProdutoHandler;

    public SearchService(IProdutoHandler produtoHandler)
    {
        ProdutoHandler = produtoHandler;
    }

    public string SearchTerm
    {
        get => _searchTerm;
        set
        {
            if (_searchTerm != value)
            {
                _searchTerm = value;
                OnSearchChanged?.Invoke();
            }
        }
    }

    public List<Produto> Produtos { get; set; } = new ();
    public List<Produto> ProdutosFiltrados { get; set; } = new();

    public async Task<Resposta<List<Produto>>> CarregarProdutos()
    {
        try
        {
            var result = await ProdutoHandler.ObterTodosProdutos();
            if (!result.IsSuccess)
                return new Resposta<List<Produto>>(null, 200, "Erro ao carregar os produtos!");
            
            Produtos = result.Dados ?? new List<Produto>();
            return new Resposta<List<Produto>>(Produtos, 200, "Produtos enviados com sucesso!");
        }
        catch (Exception e)
        {
            return new Resposta<List<Produto>>(null, 500, e.Message);
        }
    }

    public void FiltrarProdutos()
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
        {
            ProdutosFiltrados = Produtos;
        }
        else
        {
            ProdutosFiltrados = Produtos
                .Where(p => p.Titulo.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}