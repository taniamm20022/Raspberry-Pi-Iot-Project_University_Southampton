using CSDI.WebAPIClient.DataModels;
using System.Collections.Generic;


namespace LupenM.WebSite.Models
{
  public class DeviceTypesListingModel : ListingPageModel
  {
    public string SelectedUserId { get; set; }
    public List<TypeItem> Items { get; set; }
  }
}