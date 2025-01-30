using Loja.Core.Models;
using Loja.Core.Services;

namespace Loja.Api.Services;

public class ImagemService : IUploadImagemService
{
    private readonly string _diretorioImagens = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Imagens");
    
    public ImagemService()
    {
        if (!Directory.Exists(_diretorioImagens))
        {
            Directory.CreateDirectory(_diretorioImagens);
        }
    }
    
    public async Task<List<Imagem>> UploadImagem(List<IFormFile> imagens)
    {
        try
        {
            var listaDeImagens = new List<Imagem>();

            foreach (var imagem in imagens)
            {
                var extensaoValida = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extensao = Path.GetExtension(imagem.FileName)?.ToLower();

                if (!extensaoValida.Contains(extensao))
                {
                    throw new InvalidOperationException("Arquivo não é uma imagem válida.");
                }
                
                var fileName = Path.GetFileNameWithoutExtension(imagem.FileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);
            
                var filePath = Path.Combine(_diretorioImagens, fileName);
                
                var imagemUrl = Path.Combine("/Imagens", fileName).Replace("\\", "/");
                
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imagem.CopyToAsync(stream);
                }
                
                listaDeImagens.Add(new Imagem
                {
                    Url = imagemUrl
                });
            }

            return listaDeImagens;
        }
        catch (IOException ex)
        {
            Console.WriteLine(ex);
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    
    public bool ExcluirImagem(List<Imagem> imagens)
    {
        int contadorDeImagens = imagens.Count;
        int imagensExcluidas = 0;
        
        try
        {
            foreach (var imagem in imagens)
            {
                var imagemUrl = imagem.Url;

                var relativePath = imagemUrl.StartsWith("/") ? imagemUrl.Substring(1) : imagemUrl;
                relativePath = relativePath.Replace("\\", "/");
                
                if (relativePath.StartsWith("Imagens/"))
                {
                    relativePath = relativePath.Substring("Imagens/".Length);
                }

                var filePath = Path.Combine(_diretorioImagens, relativePath);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    imagensExcluidas++;
                }
            }

            return imagensExcluidas == contadorDeImagens;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao excluir a imagem: {ex.Message}");
            return false;
        }
    }
}