using CSDI.WebAPIClient;
using CSDI.WebAPIClient.DataModels;
using LupenM.WebSite.Attributes;
using LupenM.WebSite.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;



namespace LupenM.WebSite.Controllers
{
    public class DiagnosticsController : Controller
    {
        IHttpClientFactory httpClientFactory;
        private readonly TokenContainer tokenContainer;

        private readonly SensorClient sensorClient;
        private readonly DiagnosticClient diagnosticClient;
        private readonly DiagnosticClient diagnosticClient2;
        private readonly DiagnosticLogClient diagnosticLogClient;

        public DiagnosticsController()
        {
            httpClientFactory = new HttpClientFactory();
            this.tokenContainer = new TokenContainer();

            var apiClient = new ApiClient(httpClientFactory.CreateClient(), tokenContainer);
            diagnosticClient = new DiagnosticClient(apiClient);

            var apiClient2 = new ApiClient(httpClientFactory.CreateClient(), tokenContainer);
            diagnosticClient2 = new DiagnosticClient(apiClient2);

            var apiClient3 = new ApiClient(httpClientFactory.CreateClient(), tokenContainer);
            sensorClient = new SensorClient(apiClient3);

            var apiClient4 = new ApiClient(httpClientFactory.CreateClient(), tokenContainer);
            diagnosticLogClient = new DiagnosticLogClient(apiClient4);
        }

        [AuthenticationAttributeDiagnostics]
        public async Task<ActionResult> Index(DiagnosticListingPageModel model)
        {
            if (model.PageSize == 0)
            {
                model = new DiagnosticListingPageModel();
                model.PageOrder = 1;
                model.CurrentPage = 1;
                model.PageSize = 10;
            }

            var diagnosticResponse = await diagnosticClient.GetDevices(model.PageSize, model.Position);
            var result = diagnosticResponse.Data;

            foreach (var item in result.ListItems)
            {
                DateTime dateTime;

                var tryParse = DateTime.TryParse(item.Date.ToString(), out dateTime);
                var kk = DateTime.Now.Subtract(item.Interval);

                item.Status = DateTime.Compare(DateTime.Now.Subtract(item.Interval), dateTime) <= 0;
            }

            model.Items = result.ListItems;
            model.TotalRecords = result.TotalRecords;
            model.ItemsCount = model.Items.Count();

            return View(model);
        }

        [AuthenticationAttributeDiagnostics]
        public async Task<ActionResult> DiagnosticsList(DiagnosticListingPageModel model)
        {
            if (model.PageSize == 0)
            {
                model.PageOrder = 1;
                model.CurrentPage = 1;
                model.PageSize = 10;
            }

            var sensorResponse = await sensorClient.GetSensor(model.SensorId);
            var sensor = sensorResponse.Data;

            model.SensorId = sensor.SensorId;
            model.SensorName = sensor.Name;
            model.SensorTopic = sensor.Topic;

            var diagnosticResponse = await diagnosticClient.GetDiagnostics(model.SensorId, model.PageSize, model.Position);

            model.Items = diagnosticResponse.Data.ListItems;
            model.TotalRecords = diagnosticResponse.Data.TotalRecords;
            model.ItemsCount = model.Items.Count();

            return View(model);
        }

        [AuthenticationAttributeDiagnostics]
        public async Task<ActionResult> DiagnosticLogsList(DiagnosticLogListingPageModel model, int? sensorId, int? sensorDiagnosticId)
        {
            if ((model.SensorDiagnosticId == 0) && (sensorId != null))
            {
                model.SensorId = sensorId.Value;
                model.SensorDiagnosticId = sensorDiagnosticId.Value;
            }

            if (model.PageSize == 0)
            {
                model.PageOrder = 1;
                model.CurrentPage = 1;
                model.PageSize = 10;
            }

            var sensorResponse = await sensorClient.GetSensor(model.SensorId);
            var sensor = sensorResponse.Data;

            model.SensorId = sensor.SensorId;
            model.SensorName = sensor.Name;
            model.SensorTopic = sensor.Topic;
            model.UnitName = sensor.UnitName;

            var diagnosticResponse = await diagnosticLogClient.GetDiagnosticLog(model.SensorDiagnosticId, model.PageSize, model.Position);

            model.Items = diagnosticResponse.Data.ListItems;
            model.TotalRecords = diagnosticResponse.Data.TotalRecords;
            model.ItemsCount = model.Items.Count();

            return View(model);
        }

        public async Task<ActionResult> DiagnosticLogsListPartial(DiagnosticLogListingPageModel model)
        {
            if (model.PageSize == 0)
            {
                model.PageOrder = 1;
                model.CurrentPage = 1;
                model.PageSize = 10;
            }

            var sensorResponse = await sensorClient.GetSensor(model.SensorId);
            var sensor = sensorResponse.Data;

            model.SensorName = sensor.Name;
            model.SensorTopic = sensor.Topic;
            model.UnitName = sensor.UnitName;

            var diagnosticResponse = await diagnosticLogClient.GetDiagnosticLog(model.SensorDiagnosticId, 10, model.Position);

            model.Items = diagnosticResponse.Data.ListItems;
            model.TotalRecords = diagnosticResponse.Data.TotalRecords;
            model.ItemsCount = model.Items.Count();

            return PartialView("_DiagnosticLogsListPartial", model);
        }

        [AuthenticationAttributeDiagnostics]
        public async Task<ActionResult> CreateDiagnostic(int sensorId)
        {
            SensorDiagnosticItem item = new SensorDiagnosticItem();

            item.SensorId = sensorId;
            item.Date = DateTime.Now;

            var createDiagnosticResponse = await diagnosticClient.CreateSensorDiagnostic(item);

            return RedirectToAction("CreateDiagnosticLog", new DiagnosticLogListingPageModel { SensorId = sensorId, SensorDiagnosticId = createDiagnosticResponse.Data });
        }

        [AuthenticationAttributeDiagnostics]
        public async Task<ActionResult> CreateDiagnosticLog(int sensorId, int sensorDiagnosticId)
        {
            CreateDiagnosticLogModel model = new CreateDiagnosticLogModel();

            var sensorResponse = await sensorClient.GetSensor(sensorId);
            var sensor = sensorResponse.Data;

            model.SensorId = sensor.SensorId;
            model.SensorDiagnosticId = sensorDiagnosticId;
            model.IndicationInterval = sensor.IndicationInterval;
            model.Topic = sensor.Topic;
            model.Name = sensor.Name;

            ViewBag.Title = "Sensor Diagnostic";

            return View(model);
        }

        [AuthenticationAttributeDiagnostics]
        [HttpPost]
        public async Task<ActionResult> CreateDiagnosticLog(CreateDiagnosticLogModel model)
        {
            if (ModelState.IsValid)
            {
                System.Threading.Thread.Sleep(model.IndicationInterval);

                var sensorDiagnosticResponse = await diagnosticClient2.GetIndication(model.SensorId);
                var sensorDiagnostic = sensorDiagnosticResponse.Data;              
                SensorDiagnosticLogItem item = new SensorDiagnosticLogItem();
                item.SensorDiagnosticId = model.SensorDiagnosticId;
                item.ExpectedValue = model.ExpectedValue;
                item.Value = sensorDiagnostic!=null?sensorDiagnostic.LastValue:null;
                item.Date = DateTime.Now;
                item.IndicationDate = sensorDiagnostic != null ? sensorDiagnostic.Date : (DateTime?)null;
                if (sensorDiagnostic!=null && sensorDiagnostic.LastValue==model.ExpectedValue && sensorDiagnostic.Date - DateTime.Now<TimeSpan.FromMilliseconds(600))
                    item.Status = true;
                item.Status = false;
                var createDiagnosticResponse = await diagnosticLogClient.CreateSensorDiagnosticLog(item);
                return Json(new { result = "Redirect", url = Url.Action("CreateDiagnosticLog", new { sensorId = model.SensorId, sensordiagnosticId = model.SensorDiagnosticId }) });
            }
            else return View(model);
        }

        public new RedirectToRouteResult RedirectToAction(string action, string controller)
        {
            return base.RedirectToAction(action, controller);
        }
    }
}