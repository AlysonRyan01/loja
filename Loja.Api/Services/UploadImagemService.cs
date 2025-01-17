using Loja.Core.Models;
using Loja.Core.Services;

namespace Loja.Api.Services;

public class UploadImagemService : IUploadImagemService
{
    private readonly string _diretorioImagens = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Imagens");
    
    public UploadImagemService()
    {
        if (!Directory.Exists(_diretorioImagens))
        {
            Directory.CreateDirectory(_diretorioImagens);
        }
    }
    
    public async Task<List<Imagem>?> UploadImagem(List<IFormFile> imagens)
    {
        var ListaDeImagens = new List<Imagem>();
        
        if (!imagens.Any()  || string.IsNullOrWhiteSpace(imagens.First()?.FileName))
            return null;

        foreach (var imagem in imagens)
        {
            var fileName = Path.GetFileNameWithoutExtension(imagem.FileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);
            
            var filePath = Path.Combine(_diretorioImagens, fileName);
            
            var imagemUrl = Path.Combine("/Imagens", fileName);
            
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imagem.CopyToAsync(stream);
            }
            
            ListaDeImagens.Add(new Imagem
            {
                Url = imagemUrl,
            });
        }

        return ListaDeImagens;
    }
}