
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using CSDI.WebAPIClient.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSDI.WebAPIClient
{
  public class EmergencySituationsClient : ClientBase, IEmergencySituationsClient
  {
    private const string ProductUri = "api/emergencySituations";

    public EmergencySituationsClient(IApiClient apiClient) : base(apiClient)
    {
    }

    public async Task<EmergencySituationsResponse> GetEmergencySituations(DateTime? dateFrom, DateTime? dateTo, int? deviceId, int position, int pageSize)
    {
      var keyValuePair = new List<KeyValuePair<string, string>>()
      {
        new KeyValuePair<string, string>("dateFrom", dateFrom.ToString()),
        new KeyValuePair<string, string>("dateTo", dateTo.ToString()),
        new KeyValuePair<string, string>("deviceId", deviceId.ToString()),
        new KeyValuePair<string, string>("position", position.ToString()),
        new KeyValuePair<string, string>("pageSize", pageSize.ToString())
      };

      return await GetJsonDecodedContent<EmergencySituationsResponse, ListingPageModel<EmergencySituationsItem>>(ProductUri, keyValuePair.ToArray());
    }
  }
}