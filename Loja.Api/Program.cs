using Loja.Api.Data;
using Loja.Api.Handlers;
using Loja.Api.Models;
using Loja.Api.Services;
using Loja.Core;
using Loja.Core.Handlers;
using Loja.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

Configuration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? String.Empty;

builder.Services.AddDbContext<LojaDataContext>(x =>
{
    x.UseSqlite(Configuration.ConnectionString);
});
        
builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole<long>>()
    .AddEntityFrameworkStores<LojaDataContext>()
    .AddApiEndpoints();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.CustomSchemaIds(n => n.FullName); 
});

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies(); 
builder.Services.AddAuthorization();

builder.Services.AddTransient<IProdutoHandler, ProdutoHandler>();
builder.Services.AddTransient<IUploadImagemService, UploadImagemService>();

var app = builder.Build();

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
