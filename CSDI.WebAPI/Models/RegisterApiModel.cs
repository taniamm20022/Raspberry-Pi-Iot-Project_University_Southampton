
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using System.ComponentModel.DataAnnotations;

namespace CSDI.WebAPI.Models
{
  public class RegisterApiModel
  {
    [Required(ErrorMessage = "The 'Name' field is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The 'E-mail' field is required.")]
    [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "The e-mail is not valid.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required(ErrorMessage = "The 'Password' field is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "The 'Confirm password' field is required.")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    public string Telephone { get; set; }
  }
}