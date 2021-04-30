using CSDI.WebAPIClient.DataModels;
using System.Collections.Generic;



namespace LupenM.WebSite.Models
{
  public class UnitsListingModel : ListingPageModel
  {
    public string SelectedUserId { get; set; }
    public List<UnitItem> Items { get; set; }
  }
}