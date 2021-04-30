                                                                                      
using System.ComponentModel.DataAnnotations;

namespace LupenM.WebSite.Models
{
  public class CreateUnitModel
  {
    public int Id { get; set; }

    [Required(ErrorMessage = "The 'Unit' field is required.")]
    public string Name { get; set; }
  }
}