
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


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
  public class SensorsController : Controller
  {
    IHttpClientFactory httpClientFactory;
    private readonly TokenContainer tokenContainer;

    private readonly IDeviceClient deviceClient;
    private readonly UnitClient unitClient;
    private readonly SensorClient sensorClient;

    public SensorsController()
    {
      httpClientFactory = new HttpClientFactory();
      this.tokenContainer = new TokenContainer();

      var apiClient = new ApiClient(httpClientFactory.CreateClient(), tokenContainer);
      deviceClient = new DeviceClient(apiClient);

      var apiClient2 = new ApiClient(httpClientFactory.CreateClient(), tokenContainer);
      unitClient = new UnitClient(apiClient2);

      var apiClient3 = new ApiClient(httpClientFactory.CreateClient(), tokenContainer);
      sensorClient = new SensorClient(apiClient3);
    }

    [AuthenticationAttributeSensors]
    public async Task<ActionResult> Index(SensorListingModel model)
    {
      if (TempData.ContainsKey("Issuccessfull"))
      {
        model.messageModel = new MessageModel();
        model.messageModel.IsSuccessfull = (bool)TempData["Issuccessfull"];
        model.messageModel.Type = (string)TempData["Type"];
      }

      if (model.PageSize == 0)
      {
        model.PageOrder = 1;
        model.CurrentPage = 1;
        model.PageSize = 10;
      }

      var sensorResponse = await sensorClient.GetSensors(model.PageSize, model.Position);

      model.Items = sensorResponse.Data.ListItems;
      model.ItemsCount = sensorResponse.Data.ListItems.Count();
      model.TotalRecords = sensorResponse.Data.TotalRecords;

      return View(model);
    }

    [AuthenticationAttributeSensors]
    public async Task<ActionResult> CreateSensor()
    {
      var model = new CreateSensorModel();

      var devices = await deviceClient.GetDevices();
      model.ListDevices = devices.Data;

      var units = await unitClient.GetUnits();
      model.ListUnits = units.Data;

      ViewBag.Title = "Register Sensor";

      return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> CreateSensor(CreateSensorModel model)
    {
      if (ModelState.IsValid)
      {
        SensorItem item = new SensorItem();

        item.Name = model.Name;
        item.SelectedDeviceId = model.SelectedDeviceId;
        item.MinValue = model.MinValue;
        item.MaxValue = model.MaxValue;
        item.Topic = model.Topic;
        item.IndicationInterval = model.IndicationInterval.Value;
        item.SelectdUnitId = model.SelectedUnitId;
        item.Active = model.Active;
        item.Date = DateTime.Now;
        item.Location = model.Location;

        var response = await sensorClient.CreateSensor(item);

        TempData["Issuccessfull"] = response.StatusIsSuccessful;
        TempData["Type"] = "Save";

        return RedirectToAction("Index");
      }

      return View(model);
    }

    public async Task<ActionResult> UpdateSensor(int Id)
    {
      ViewBag.Title = "Update Sensor";

      var sensorResponse = await sensorClient.GetSensor(Id);
      var sensor = sensorResponse.Data;

      var model = new CreateSensorModel()
      {
        Id = sensor.SensorId,
        Active = sensor.Active,
        MaxValue = sensor.MaxValue,
        MinValue = sensor.MinValue,
        Name = sensor.Name,
        Topic = sensor.Topic,
        IndicationInterval = sensor.IndicationInterval,
        SelectedDeviceId = sensor.SelectedDeviceId,
        SelectedUnitId = sensor.SelectdUnitId,
        Location = sensor.Location
      };// Create ViewModel via some kind of mapping (check the accompanying VS solution to see this in action)

      var devices = await deviceClient.GetDevices();
      var units = await unitClient.GetUnits();

      model.ListUnits = units.Data;
      model.ListDevices = devices.Data;

      return View("CreateSensor", model);
    }

    [AuthenticationAttribute]
    [HttpPost]
    public async Task<ActionResult> UpdateSensor(CreateSensorModel model)
    {
      //isvalid
      SensorItem item = new SensorItem();

      item.SensorId = model.Id;
      item.Active = model.Active;
      item.MaxValue = model.MaxValue;
      item.MinValue = model.MinValue;
      item.Name = model.Name;
      item.Topic = model.Topic;
      item.IndicationInterval = model.IndicationInterval.Value;
      item.SelectedDeviceId = model.SelectedDeviceId;
      item.SelectdUnitId = model.SelectedUnitId;
      item.Location = model.Location;

      var response = await sensorClient.UpdateSensor(item);
      var productId = response.Data;

      TempData["Issuccessfull"] = response.StatusIsSuccessful;
      TempData["Type"] = "Update";

      return RedirectToAction("Index");
    }

    public async Task<ActionResult> DeleteSensor(int id)
    {
      var response = await sensorClient.DeleteSensor(id);

      TempData["Issuccessfull"] = response.StatusIsSuccessful;
      TempData["Type"] = "Delete";

      return RedirectToAction("Index");
    }

    public new RedirectToRouteResult RedirectToAction(string action, string controller)
    {
      return base.RedirectToAction(action, controller);
    }
  }
}