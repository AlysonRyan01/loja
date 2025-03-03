using System.Text.Json;
using Loja.Api.Data;
using Loja.Api.Handlers;
using Loja.Api.Services;
using Loja.Core;
using Loja.Core.Handlers;
using Loja.Core.Models.Identity;
using Loja.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Loja.Api.Common;

public static class BuilderExtension
{
    public static void AddConfigurationApiUrl(this WebApplicationBuilder builder)
    {
        Configuration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? String.Empty;
        Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? String.Empty;
        Configuration.FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? String.Empty;
    }
    
    public static void AddCorsConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options => options.AddPolicy(
            ApiConfiguration.CorsPolicyName,
            policy => policy
                .WithOrigins(
                [
                    Configuration.BackendUrl,
                    Configuration.FrontendUrl
                ])
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
        ));
    }
    
    public static void AddRootPath(this WebApplicationBuilder builder)
    {
        builder.Environment.ContentRootPath = Directory.GetCurrentDirectory();
        builder.Environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    }
    
    public static void AddControllers(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true; 
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; 
            });
    }
    
    public static void AddDbConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<LojaDataContext>(x =>
        {
            x.UseSqlite(Configuration.ConnectionString);
        });
    }
    
    public static void AddIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentityCore<User>()
            .AddRoles<IdentityRole<long>>()
            .AddEntityFrameworkStores<LojaDataContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
    }
    
    public static void AddSecurity(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddIdentityCookies();

        builder.Services.AddAuthorization();
    }
    
    public static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Cookie", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Cookie,
                Name = "YourAppCookie",
                Scheme = "cookie"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Cookie"
                        }
                    },
                    new string[] {}
                }
            });
        });
    }
    
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IProdutoHandler, ProdutoHandler>();
        builder.Services.AddTransient<IEnderecoHandler, EnderecoHandler>();
        builder.Services.AddTransient<IIdentityHandler, IdentityHandler>();
        builder.Services.AddTransient<ICarrinhoItemHandler, CarrinhoItemHandler>();
        builder.Services.AddTransient<ICarrinhoHandler, CarrinhoHandler>();
        builder.Services.AddTransient<IPedidoHandler, PedidoHandler>();
        builder.Services.AddTransient<ICorreioHandler, CorreioHandler>();
        builder.Services.AddTransient<EmailService>();
        builder.Services.AddTransient<IUploadImagemService, ImagemService>();
        builder.Services.AddTransient<IPedidoItemService, PedidoItemService>();
        builder.Services.AddHttpClient();
    }
}