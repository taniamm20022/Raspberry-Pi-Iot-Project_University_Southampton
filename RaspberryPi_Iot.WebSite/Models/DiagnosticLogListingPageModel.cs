using CSDI.WebAPIClient.DataModels;
using System.Collections.Generic;

namespace LupenM.WebSite.Models
{
  public class DiagnosticLogListingPageModel : ListingPageModel
  {
    public int SensorDiagnosticId { get; set; }
    public int SensorId { get; set; }
    public List<SensorDiagnosticLogItem> Items { get; set; }
    public string SensorName { get; internal set; }
    public string SensorTopic { get; internal set; }
    public string UnitName { get; set; }
  }
}