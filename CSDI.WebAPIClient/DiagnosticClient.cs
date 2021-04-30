using CSDI.WebAPIClient.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSDI.WebAPIClient
{
  public class DiagnosticClient : ClientBase
  {

    private const string ProductUri = "api/diagnostics";

    public DiagnosticClient(IApiClient apiClient) : base(apiClient)
    {
    }

    public async Task<DiagnosticsResponse> GetDevices(int pageSize, int position)
    {
      KeyValuePair<string, string>[] keyValuePair = new KeyValuePair<string, string>[]
      {
        new KeyValuePair<string, string>("pageSize", pageSize.ToString() ),
        new KeyValuePair<string, string>("position", position.ToString() )
      };

      return await GetJsonDecodedContent<DiagnosticsResponse, ListingPageModel<DiagnosticItem>>(ProductUri, keyValuePair);
    }

    public async Task<DiagnosticResponse> GetIndication(int sensorId)
    {
      var keyValuePair = new List<KeyValuePair<string, string>>()
      {
        new KeyValuePair<string, string>("sensorId", sensorId.ToString())
      };

      return await GetJsonDecodedContent<DiagnosticResponse, DiagnosticItem>(ProductUri, keyValuePair.ToArray());
    }

    public async Task<DiagnosticsResponse> GetDiagnostics(int sensorId, int pageSize, int position)
    {
      var keyValuePair = new List<KeyValuePair<string, string>>()
      {
        new KeyValuePair<string, string>("sensorid", sensorId.ToString()),
        new KeyValuePair<string, string>("pageSize", pageSize.ToString()),
        new KeyValuePair<string, string>("position", position.ToString())
      };

      return await GetJsonDecodedContent<DiagnosticsResponse, ListingPageModel<DiagnosticItem>>(ProductUri, keyValuePair.ToArray());
    }

    public async Task<CreateResponse> CreateSensorDiagnostic(SensorDiagnosticItem item)
    {
      var createSensorDiagnosticResponse = await PostEncodedContentWithSimpleResponse<CreateResponse, SensorDiagnosticItem>(ProductUri, item);

      return createSensorDiagnosticResponse;
    }
  }
}