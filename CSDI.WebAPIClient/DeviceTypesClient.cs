using CSDI.WebAPIClient.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSDI.WebAPIClient
{
  public class DeviceTypesClient : ClientBase
  {
    private const string ProductUri = "api/deviceTypes";

    public DeviceTypesClient(IApiClient apiClient) : base(apiClient)
    {
    }

    public async Task<DeviceTypesListResponse> GetDeviceTypess(string userIdFilter, int pageSize, int position)
    {
      KeyValuePair<string, string>[] keyValuePair = new KeyValuePair<string, string>[]
      {
          new KeyValuePair<string, string>("useridfilter", userIdFilter),
          new KeyValuePair<string, string>("pageSize", pageSize.ToString()),
          new KeyValuePair<string, string>("position", position.ToString())
      };

      return await GetJsonDecodedContent<DeviceTypesListResponse, ListingPageModel<TypeItem>>(ProductUri, keyValuePair);
    }

    public async Task<DeviceTypesResponse> GetDeviceTypes()
    {
      KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>();

      return await GetJsonDecodedContent<DeviceTypesResponse, List<TypeItem>>(ProductUri, keyValuePair);
    }

    public async Task<CreateResponse> CreateDeviceType(TypeItem item)
    {
      var createProductResponse = await PostEncodedContentWithSimpleResponse<CreateResponse, TypeItem>(ProductUri, item);

      return createProductResponse;
    }

    public async Task<DeviceTypeResponse> GetDeviceType(int id)
    {
      var idPair = new KeyValuePair<string, string>("id", id.ToString());

      return await GetJsonDecodedContent<DeviceTypeResponse, TypeItem>(ProductUri, idPair);
    }

    public async Task<CreateResponse> UpdateDeviceType(TypeItem item)
    {
      var updateProductResponse = await PutEncodedContentWithSimpleResponse<CreateResponse, TypeItem>(ProductUri, item);

      return updateProductResponse;
    }

    public async Task<CreateResponse> DeleteDeviceType(int id)
    {
      var idPair = new KeyValuePair<string, string>("id", id.ToString());

      return await DeleteFormEncodedContentWithSimpleResponse<CreateResponse>(ProductUri, idPair);
    }
  }
}