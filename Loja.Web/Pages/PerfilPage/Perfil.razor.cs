using System.Security.Claims;
using Loja.Core.Handlers;
using Loja.Core.Models.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.PerfilPage;

public partial class PerfilPage : ComponentBase
{
    #region Propriedades

    public MudForm UserInfoForm;
    public bool UserInfoIsValid { get; set; } = false;
    
    public UserInfo UserInfo { get; set; } = new();
    public bool IsBusy { get; set; } = false;

    #endregion
    
    #region Injectables
    
    [Inject] private IIdentityHandler IdentityHandler { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var result = await IdentityHandler.UserInfo(null);

            if (result.IsSuccess)
            {
                UserInfo = result.Dados ?? new();
            }
            else
            {
                Snackbar.Add("Erro ao obter as suas informações...", Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add("Erro no servidor...", Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion
    
    public string ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return "O telefone é obrigatório";
        
        if (phone.Length != 11)
            return "O telefone precisa ter 11 números";
        
        return null;
    }
    
    public string ValidateCEP(string cep)
    {
        if (string.IsNullOrWhiteSpace(cep))
            return "O campo CEP é obrigatório";
        
        string cepSemHifen = cep.Replace("-", "");
        
        if (cepSemHifen.Length != 8)
            return "O CEP deve conter 8 caracteres.";
        
        return null;
    }

    public void FormatPhoneNumber()
    {
        if (!string.IsNullOrWhiteSpace(UserInfo.PhoneNumber))
        {
            var cleanPhone = new string(UserInfo.PhoneNumber.Where(char.IsDigit).ToArray());
            
            if (cleanPhone.Length == 11)
            {
                UserInfo.PhoneNumber = string.Format("({0}) {1}-{2}",
                    cleanPhone.Substring(0, 2),
                    cleanPhone.Substring(2, 5),
                    cleanPhone.Substring(7, 4));
            }
        }
    }
}