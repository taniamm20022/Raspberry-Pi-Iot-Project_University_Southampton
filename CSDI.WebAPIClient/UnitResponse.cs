
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using CSDI.WebAPIClient.DataModels;
using System.Collections.Generic;

namespace CSDI.WebAPIClient
{
  public class UnitResponse : ApiResponse<UnitItem>
  {
  }

  public class UnitsListResponse : ApiResponse<ListingPageModel<UnitItem>>
  {
  }

  public class UnitsResponse : ApiResponse<List<UnitItem>>
  {
  }
}