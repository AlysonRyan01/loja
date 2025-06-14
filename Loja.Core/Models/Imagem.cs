﻿using System.Text.Json.Serialization;

namespace Loja.Core.Models;

public class Imagem
{
    public long Id { get; set; }
    public string Url { get; set; }
    public long ProdutoId { get; set; } 
    
    [JsonIgnore]
    public Produto Produto { get; set; }
}