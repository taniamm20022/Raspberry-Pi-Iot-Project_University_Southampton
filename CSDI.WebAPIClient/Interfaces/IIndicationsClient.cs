
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using System;
using System.Threading.Tasks;

namespace CSDI.WebAPIClient
{
  public interface IIndicationsClient
  {
    Task<IndicationsResponse> GetIndications(DateTime? dateFrom, DateTime? dateTo, int? deviceId, int position, int pageSize);
  }
}