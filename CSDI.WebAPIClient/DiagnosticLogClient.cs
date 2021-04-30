using CSDI.WebAPIClient.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSDI.WebAPIClient
{
  public class DiagnosticLogClient : ClientBase
  {

    private const string ProductUri = "api/diagnosticLog";

    public DiagnosticLogClient(IApiClient apiClient) : base(apiClient)
    {
    }

    public async Task<DiagnosticLogResponse> GetDiagnosticLog(int sensorDiagnosticId, int pageSize, int position)
    {
      var keyValuePair = new List<KeyValuePair<string, string>>()
      {
        new KeyValuePair<string, string>("sensordiagnosticid", sensorDiagnosticId.ToString()),
        new KeyValuePair<string, string>("pagesize", pageSize.ToString()),
        new KeyValuePair<string, string>("position", position.ToString())
      };

      return await GetJsonDecodedContent<DiagnosticLogResponse, ListingPageModel<SensorDiagnosticLogItem>>(ProductUri, keyValuePair.ToArray());
    }
    public async Task<CreateResponse> CreateSensorDiagnosticLog(SensorDiagnosticLogItem item)
    {
      var createSensorDiagnosticResponse = await PostEncodedContentWithSimpleResponse<CreateResponse, SensorDiagnosticLogItem>(ProductUri, item);

      return createSensorDiagnosticResponse;
    }
  }
}
