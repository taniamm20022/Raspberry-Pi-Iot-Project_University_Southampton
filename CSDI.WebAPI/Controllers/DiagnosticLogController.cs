using CSDI.Data;
using CSDI.Data.Models;
using CSDI.WebAPI.Models;
using System.Collections.Generic;
using System.Web.Http;


namespace LupenM.WebAPI.Controllers
{
    public class DiagnosticLogController : ApiController
    {
        public int Post(SensorDiagnosticLogItem item)
        {
            int result = DiagnosticsData.AddDiagnosticLog(new SensorDiagnosticLog
            {
                SensorDiagnosticId = item.SensorDiagnosticId,
                ExpectedValue = item.ExpectedValue,
                IndicationDate=item.IndicationDate,
                Value = item.Value,
                Date = item.Date
            });

            return result;
        }

        public ListingPageModel<SensorDiagnosticLogItem> Get(int sensorDiagnosticId, int pageSize, int position)
        {
            ListingPageModel<SensorDiagnosticLogItem> result = new ListingPageModel<SensorDiagnosticLogItem>();

            int totalRecords;
            List<SensorDiagnosticLog> lstSensorDiagmosticLogs = DiagnosticsData.GetSensorDiagnosticLogs(sensorDiagnosticId, position, pageSize, out totalRecords);

            foreach (var sensorDiagnosticLog in lstSensorDiagmosticLogs)
            {
                result.ListItems.Add(new SensorDiagnosticLogItem
                {
                    SensorDiagnosticId = sensorDiagnosticLog.SensorDiagnosticId,
                    Date = sensorDiagnosticLog.Date,
                    IndicationDate = sensorDiagnosticLog.IndicationDate,
                    ExpectedValue = sensorDiagnosticLog.ExpectedValue,
                    Value = sensorDiagnosticLog.Value,
                    Status = sensorDiagnosticLog.Status
                });
            }

            result.TotalRecords = totalRecords;

            return result;
        }
    }
}