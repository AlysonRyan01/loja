using System.Security.Claims;
using System.Text.RegularExpressions;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Models.Identity;
using Loja.Core.Requisicoes.Endereco;
using Loja.Core.Requisicoes.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.PerfilPage;

public partial class PerfilPage : ComponentBase
{
    #region Propriedades

    public MudForm UserEnderecoForm;
    public MudForm UserInfoForm;
    
    public UserInfo UserInfo { get; set; } = new();
    public Endereco Endereco { get; set; } = new();
    public string Username { get; set; } = string.Empty;
    
    public bool IsBusy { get; set; } = false;
    
    public bool EnderecoIsBusy { get; set; } = false;
    public bool EnderecoIsValid { get; set; } = false;
    
    public bool UserInfoIsBusy { get; set; } = false;
    public bool UserInfoIsValid { get; set; } = false;

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
                Username = UserInfo.FullName;
                Endereco = result.Dados.Endereco ?? new();
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
        string phoneReal = Regex.Replace(phone, @"\D", "");
        
        if (string.IsNullOrWhiteSpace(phoneReal))
            return "O telefone é obrigatório";
        
        if (phoneReal.Length != 11)
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
    
    public PatternMask CepMask = new PatternMask("00000-000")
    {
        MaskChars = new[] { new MaskChar('0', @"[0-9]") }
    };
    
    public async Task UserInfoValidation()
    {
        UserInfoIsBusy = true;
        try
        {
            await UserInfoForm.Validate();

            if (!UserInfoForm.IsValid)
            {
                Console.WriteLine("Preencha o formulario corretamente");
                return;
            }

            var request = new UserInfoValidationRequest
            {
                UserId = UserInfo.Id,
                Email = UserInfo.Email,
                FullName = UserInfo.FullName,
                PhoneNumber = Regex.Replace(UserInfo.PhoneNumber, @"\D", "")
            };

            var result = await IdentityHandler.UserInfoValidation(request);

            if (result.IsSuccess)
            {
                UserInfoIsValid = true;
                Snackbar.Add("Dados atualizados com sucesso!", Severity.Success);
                StateHasChanged();
            }
            else
            {
                Snackbar.Add("Erro ao atualizar os dados..", Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
        finally
        {
            UserInfoIsBusy = false;
        }
    }

    public async Task AtualizarEndereco()
    {
        EnderecoIsBusy = true;
        try
        {
            await UserEnderecoForm.Validate();
            
            if (!UserEnderecoForm.IsValid)
                Snackbar.Add("Preencha todos os campos corretamente", Severity.Error);
            
            if (new[] { Endereco.CEP, Endereco.Rua, Endereco.Bairro, Endereco.Cidade, Endereco.Estado, Endereco.Numero }
                .Any(valor => !string.IsNullOrEmpty(valor)))
            {
                var endereco = new AtualizarEnderecoRequisicao
                {
                    UserId = UserInfo.Id,
                    Rua = Endereco.Rua,
                    Numero = Endereco.Numero,
                    Bairro = Endereco.Bairro,
                    Cidade = Endereco.Cidade,
                    Estado = Endereco.Estado,
                    CEP = Endereco.CEP.Replace("-", "") ?? string.Empty,
                    Pais = "Brasil"
                };

                var result = await IdentityHandler.UserAdressValidation(endereco);

                if (result.IsSuccess)
                {
                    Snackbar.Add("Endereço atualizado com sucesso!", Severity.Success);
                    EnderecoIsValid = true;
                    StateHasChanged();
                }
                else
                    Snackbar.Add("Erro ao atualizar o endereço", Severity.Error);
            }
            else
                Snackbar.Add("Preencha todos os campos do endereço", Severity.Error);
        }
        catch
        {
            Snackbar.Add("Erro ao atualizar o endereço...", Severity.Error);
        }
        finally
        {
            EnderecoIsBusy = false;
        }
    }
}