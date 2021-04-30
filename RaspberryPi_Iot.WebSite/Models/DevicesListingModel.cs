
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using CSDI.WebAPIClient.DataModels;
using System.Collections.Generic;


namespace LupenM.WebSite.Models
{
  public class DevicesListingModel : ListingPageModel
  {
    public List<UsersItem> UserItems { get; set; }
    public string SelectedUserId { get; set; }

    public List<DeviceItem> Items { get; set; }
    public int SelectedUserItemId { get; set; }
  }
}