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
  public class SensorClient : ClientBase
  {
    private const string ProductUri = "api/sensors";

    public SensorClient(IApiClient apiClient) : base(apiClient)
    {
    }

    public async Task<SensorsResponse> GetSensors(int pageSize, int position)
    {
      KeyValuePair<string, string>[] keyValuePair = new KeyValuePair<string, string>[]
      {
          new KeyValuePair<string, string>("pageSize", pageSize.ToString()),
          new KeyValuePair<string, string>("position", position.ToString())
      };

      return await GetJsonDecodedContent<SensorsResponse, ListingPageModel<SensorItem>>(ProductUri, keyValuePair);
    }

    public async Task<SensorResponse> GetSensor(int id)
    {
      var idPair = new KeyValuePair<string, string>("id", id.ToString());

      return await GetJsonDecodedContent<SensorResponse, SensorItem>(ProductUri, idPair);
    }

    public async Task<CreateResponse> CreateSensor(SensorItem sensorItem)
    {
      var createSensorResponse = await PostEncodedContentWithSimpleResponse<CreateResponse, SensorItem>(ProductUri, sensorItem);

      return createSensorResponse;
    }

    public async Task<CreateResponse> UpdateSensor(SensorItem sensorItem)
    {
      var updateSensorResponse = await PutEncodedContentWithSimpleResponse<CreateResponse, SensorItem>(ProductUri, sensorItem);

      return updateSensorResponse;
    }

    public async Task<CreateResponse> DeleteSensor(int id)
    {
      var idPair = new KeyValuePair<string, string>("id", id.ToString());

      return await DeleteFormEncodedContentWithSimpleResponse<CreateResponse>(ProductUri, idPair);
    }
  }
}