
using System.Collections.Generic;

namespace CSDI.WebAPIClient.DataModels
{
  public class ListingPageModel<T>
  {
    public ListingPageModel()
    {
      this.ListItems = new List<T>();
    }

    public List<T> ListItems { get; set; }
    public int TotalRecords { get; set; }
  }
}