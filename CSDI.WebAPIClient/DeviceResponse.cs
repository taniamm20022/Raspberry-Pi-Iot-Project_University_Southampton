
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using System.Collections.Generic;

namespace CSDI.WebAPIClient.DataModels
{
  public class DeviceResponse : ApiResponse<DeviceItem>
  {
  }

  public class DevicesResponse : ApiResponse<List<DeviceItem>>
  {
  }

  public class DevicesListResponse : ApiResponse<ListingPageModel<DeviceItem>>
  {
  }
}