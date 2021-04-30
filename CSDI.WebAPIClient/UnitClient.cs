
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using CSDI.WebAPIClient.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSDI.WebAPIClient
{
  public class UnitClient : ClientBase
  {
    private const string ProductUri = "api/units";

    public UnitClient(IApiClient apiClient) : base(apiClient)
    {
    }

    public async Task<UnitsListResponse> GetUnits(string userIdFilter, int pageSize, int position)
    {
      KeyValuePair<string, string>[] keyValuePair = new KeyValuePair<string, string>[]
      {
          new KeyValuePair<string, string>("useridfilter", userIdFilter),
          new KeyValuePair<string, string>("pageSize", pageSize.ToString()),
          new KeyValuePair<string, string>("position", position.ToString())
      };

      return await GetJsonDecodedContent<UnitsListResponse, ListingPageModel<UnitItem>>(ProductUri, keyValuePair);
    }

    public async Task<UnitsResponse> GetUnits()
    {
      KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>();

      return await GetJsonDecodedContent<UnitsResponse, List<UnitItem>>(ProductUri, keyValuePair);
    }

    public async Task<CreateResponse> CreateUnit(UnitItem item)
    {
      var createProductResponse = await PostEncodedContentWithSimpleResponse<CreateResponse, UnitItem>(ProductUri, item);

      return createProductResponse;
    }

    public async Task<UnitResponse> GetUnit(int id)
    {
      var idPair = new KeyValuePair<string, string>("id", id.ToString());

      return await GetJsonDecodedContent<UnitResponse, UnitItem>(ProductUri, idPair);
    }

    public async Task<CreateResponse> UpdateUnit(UnitItem item)
    {
      var updateProductResponse = await PutEncodedContentWithSimpleResponse<CreateResponse, UnitItem>(ProductUri, item);

      return updateProductResponse;
    }

    public async Task<CreateResponse> DeleteUnit(int id)
    {
      var idPair = new KeyValuePair<string, string>("id", id.ToString());

      return await DeleteFormEncodedContentWithSimpleResponse<CreateResponse>(ProductUri, idPair);
    }
  }
}