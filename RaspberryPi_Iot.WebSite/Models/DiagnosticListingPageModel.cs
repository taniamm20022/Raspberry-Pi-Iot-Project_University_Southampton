using CSDI.WebAPIClient.DataModels;
using System.Collections.Generic;

namespace LupenM.WebSite.Models
{
  public class DiagnosticListingPageModel : ListingPageModel
  {
    public int SensorId { get; set; }
    public List<DiagnosticItem> Items { get; set; }
    public string SensorName { get; internal set; }
    public string SensorTopic { get; internal set; }
    public string UnitName { get; set; }
  }
}