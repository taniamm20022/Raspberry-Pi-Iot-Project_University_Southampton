
using System;
using System.ComponentModel.DataAnnotations;


namespace LupenM.WebSite.Models
{
  public class CreateDiagnosticLogModel
  {
    public int SensorDiagnosticId { get; set; }
    public int SensorId { get; set; }

    [Required(ErrorMessage = "The 'Expected Value' field is required.")]
    public string ExpectedValue { get; set; }
    public string Topic { get; set; }
    public string Name { get; set; }

    [Required(ErrorMessage = "The 'Indication interval' field is required.")]
    [DisplayFormat(DataFormatString = @"{0:hh\:mm\:ss}", ApplyFormatInEditMode = true)]
    [RegularExpression(@"((([0-1][0-9])|(2[0-3]))(:[0-5][0-9])(:[0-5][0-9])?)", ErrorMessage = "Indication interval must be hh:mm:ss")]
    public TimeSpan IndicationInterval { get; set; }
  }
}