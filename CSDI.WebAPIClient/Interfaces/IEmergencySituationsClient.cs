
using System;
using System.Threading.Tasks;

namespace CSDI.WebAPIClient
{
  public interface IEmergencySituationsClient
  {
    Task<EmergencySituationsResponse> GetEmergencySituations(DateTime? dateFrom, DateTime? dateTo, int? deviceId, int position, int pageSize);
  }
}