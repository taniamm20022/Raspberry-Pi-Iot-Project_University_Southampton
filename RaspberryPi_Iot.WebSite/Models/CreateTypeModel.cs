using System.ComponentModel.DataAnnotations;


namespace LupenM.WebSite.Models
{
  public class CreateTypeModel
  {
    public int Id { get; set; }

    [Required(ErrorMessage = "The 'Device name' field is required.")]
    public string Name { get; set; }
  }
}