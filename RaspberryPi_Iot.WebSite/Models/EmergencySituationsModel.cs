using CSDI.WebAPIClient.DataModels;
using System;
using System.Collections.Generic;

namespace LupenM.WebSite.Models
{
  public class EmergencySituationsModel : ListingPageModel
  {
    public EmergencySituationsModel()
    {
      this.ListDevices = new List<DeviceItem>();
    }

    public List<EmergencySituationsItem> EmergencySituations { get; set; }

    //filters
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public int? SelectedDeviceId { get; set; }
    public List<DeviceItem> ListDevices { get; set; }
  }
}