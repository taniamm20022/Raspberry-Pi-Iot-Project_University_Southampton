
using CSDI.WebAPIClient.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSDI.WebAPIClient
{
  public class DeviceClient : ClientBase, IDeviceClient
  {
    private const string ProductUri = "api/devices";

    public DeviceClient(IApiClient apiClient) : base(apiClient)
    {
    }

    public async Task<DevicesResponse> GetDevices()
    {
      KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>();

      return await GetJsonDecodedContent<DevicesResponse, List<DeviceItem>>(ProductUri, keyValuePair);
    }

    public async Task<DevicesListResponse> GetDevices(string userIdFilter, int pageSize, int position)
    {
      KeyValuePair<string, string>[] keyValuePair = new KeyValuePair<string, string>[]
      {
          new KeyValuePair<string, string>("useridfilter", userIdFilter),
          new KeyValuePair<string, string>("pagesize", pageSize.ToString()),
          new KeyValuePair<string, string>( "position", position.ToString())
      };

      return await GetJsonDecodedContent<DevicesListResponse, ListingPageModel<DeviceItem>>(ProductUri, keyValuePair);
    }

    public async Task<DeviceResponse> GetDevice(int productId)
    {
      var idPair = new KeyValuePair<string, string>("id", productId.ToString());

      return await GetJsonDecodedContent<DeviceResponse, DeviceItem>(ProductUri, idPair);
    }

    public async Task<CreateResponse> DeleteDevice(int deviceId)
    {
      var idPair = new KeyValuePair<string, string>("id", deviceId.ToString());

      return await DeleteFormEncodedContentWithSimpleResponse<CreateResponse>(ProductUri, idPair);
    }

    public async Task<CreateResponse> CreateDevice(DeviceItem deviceItem)
    {
      var createProductResponse = await PostEncodedContentWithSimpleResponse<CreateResponse, DeviceItem>(ProductUri, deviceItem);

      return createProductResponse;
    }

    public async Task<CreateResponse> UpdateDevice(DeviceItem deviceItem)
    {
      var updateProductResponse = await PutEncodedContentWithSimpleResponse<CreateResponse, DeviceItem>(ProductUri, deviceItem);

      return updateProductResponse;
    }
  }
}