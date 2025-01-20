using Loja.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Loja.Core.Services;

public interface IUploadImagemService
{
    Task<List<Imagem>> UploadImagem(List<IFormFile> imagens);
}