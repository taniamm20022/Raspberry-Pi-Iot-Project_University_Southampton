using CSDI.WebAPIClient.DataModels;
using System.Collections.Generic;

namespace LupenM.WebSite.Models
{
  public class UserListingPageModel : ListingPageModel
  {
    public List<UsersItem> Items { get; set; }
  }
}