using CSDI.WebAPIClient.DataModels;

using System.Collections.Generic;

/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


namespace LupenM.WebSite.Models
{
  public class SensorListingModel : ListingPageModel
  {
    public string SelectedUserId { get; set; }
    public List<SensorItem> Items { get; set; }
  }
}