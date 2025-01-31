using Loja.Api;
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

var builder = WebApplication.CreateBuilder(args);

Configuration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? String.Empty;
Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? String.Empty;
Configuration.FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? String.Empty;

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

builder.Environment.ContentRootPath = Directory.GetCurrentDirectory();
builder.Environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

builder.Services.AddControllers();

builder.Services.AddDbContext<LojaDataContext>(x =>
{
    x.UseSqlite(Configuration.ConnectionString);
});



builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole<long>>()
    .AddEntityFrameworkStores<LojaDataContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.UserNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
});

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

builder.Services.AddAuthorization();

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

builder.Services.AddTransient<IProdutoHandler, ProdutoHandler>();
builder.Services.AddTransient<IIdentityHandler, IdentityHandler>();
builder.Services.AddTransient<IUploadImagemService, ImagemService>();
builder.Services.AddTransient<ICarrinhoItemHandler, CarrinhoItemHandler>();
builder.Services.AddTransient<ICarrinhoHandler, CarrinhoHandler>();

var app = builder.Build();

app.UseCors(ApiConfiguration.CorsPolicyName);

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");

app.Run();
