
using CSDI.WebAPIClient;
using CSDI.WebAPIClient.DataModels;
using System.Threading.Tasks;

namespace CSDI.WebAPIClient
{
  public interface IDeviceClient
  {
    Task<DevicesResponse> GetDevices();
    Task<DevicesListResponse> GetDevices(string userIdFilter, int pageSize, int position);
    Task<DeviceResponse> GetDevice(int productId);
    Task<CreateResponse> CreateDevice(DeviceItem device);
    Task<CreateResponse> UpdateDevice(DeviceItem device);
    Task<CreateResponse> DeleteDevice(int deviceId);
  }
}