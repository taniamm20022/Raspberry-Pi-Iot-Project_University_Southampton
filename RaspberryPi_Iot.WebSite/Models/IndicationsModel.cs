using CSDI.WebAPIClient.DataModels;
using System;
using System.Collections.Generic;

namespace LupenM.WebSite.Models
{
  public class IndicationsModel : ListingPageModel
  {
    public IndicationsModel()
    {
      this.ListDevices = new List<DeviceItem>();
    }

    public List<IndicationsItem> Indications { get; set; }

    //filters
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public int? SelectedDeviceId { get; set; }
    public List<DeviceItem> ListDevices { get; set; }
  }
}