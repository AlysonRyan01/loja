using Loja.Api.Common;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfigurationApiUrl();
builder.AddCorsConfiguration();
builder.AddControllers();
builder.AddRootPath();
builder.AddDbConfiguration();
builder.AddIdentity();
builder.AddSecurity();
builder.AddSwagger();
builder.AddServices();

var app = builder.Build();

app.AddCors();
app.AddSmtpConfiguration();
app.AddRouteConfiguration();
app.AddSecurity();
app.MapControllers();
app.AddSwagger();

app.MapGet("/", () => "Api rodando");

app.Run();
