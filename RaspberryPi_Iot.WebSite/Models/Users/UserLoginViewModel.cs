
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using System.ComponentModel.DataAnnotations;

namespace LupenM.WebSite.Models.Users
{
  public class UserLoginViewModel
  {
    public UserLoginViewModel() { }

    [Required(ErrorMessage = "[UserManagement].[Login_UserNameRequired]")]
    [EmailAddress(ErrorMessage = "UserManagement.Register_InvalidEmailValidator")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "UserManagement.Login_PasswordRequired")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
  }
}