namespace Loja.Api.Common;

public static class AppExtension
{
    public static void AddCors(this WebApplication app)
    {
        app.UseCors(ApiConfiguration.CorsPolicyName);
    }
    
    public static void AddSmtpConfiguration(this WebApplication app)
    {
        var smtp = new ApiConfiguration.SmtpConfiguration();
        app.Configuration.GetSection("Smtp").Bind(smtp);
        ApiConfiguration.Smtp = smtp;
    }
    
    public static void AddRouteConfiguration(this WebApplication app)
    {
        app.UseStaticFiles();
        app.UseHttpsRedirection();
        app.UseRouting();
    }
    
    public static void AddSecurity(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
    
    public static void AddSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}