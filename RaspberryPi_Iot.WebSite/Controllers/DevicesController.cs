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
  public class DeviceController : Controller
  {
    IHttpClientFactory httpClientFactory;
    private readonly TokenContainer tokenContainer;

    private readonly IDeviceClient deviceClient;
    private readonly AccountClient accountClient;
    private readonly DeviceTypesClient deviceTypeClient;

    public DeviceController()
    {
      httpClientFactory = new HttpClientFactory();
      this.tokenContainer = new TokenContainer();

      var apiClient = new ApiClient(httpClientFactory.CreateClient(), tokenContainer);
      deviceClient = new DeviceClient(apiClient);

      var apiClient2 = new ApiClient(httpClientFactory.CreateClient(), tokenContainer);
      accountClient = new AccountClient(apiClient2);

      var apiClient3 = new ApiClient(httpClientFactory.CreateClient(), tokenContainer);
      deviceTypeClient = new DeviceTypesClient(apiClient3);
    }

    public DeviceController(IDeviceClient devClient)
    {
      this.deviceClient = devClient;
    }

    public new RedirectToRouteResult RedirectToAction(string action, string controller)
    {
      return base.RedirectToAction(action, controller);
    }

    [AuthenticationAttribute]
    public async Task<ActionResult> Index(DevicesListingModel model)
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

      var devices = await deviceClient.GetDevices(model.SelectedUserId, model.PageSize, model.Position);

      model.Items = devices.Data.ListItems;
      model.TotalRecords = devices.Data.TotalRecords;
      model.ItemsCount = model.Items.Count();

      if (tokenContainer.UserRole.ToString() != "users")
      {
        var usersResponse = await accountClient.GetUsers();
        model.UserItems = usersResponse.Data;
      }

      ViewBag.UserRole = tokenContainer.UserRole.ToString();

      return View(model);
    }

    [AuthenticationAttribute]
    public async Task<ActionResult> CreateDevice()
    {
      var model = new CreateDeviceModel();
      var deviceTypes = await deviceTypeClient.GetDeviceTypes();

      model.ListDeviceTypes = deviceTypes.Data;

      if (tokenContainer.UserRole.ToString() != "users")
      {
        var usersResponse = await accountClient.GetUsers();
        model.ListUsers = usersResponse.Data;
      }

      ViewBag.UserRole = tokenContainer.UserRole.ToString();
      ViewBag.Title = "Register Device";

      return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> CreateDevice(CreateDeviceModel model)
    {
      if (ModelState.IsValid)
      {
        DeviceItem item = new DeviceItem();

        item.Date = DateTime.Now;
        item.Active = model.Active;
        item.IP = model.IP;
        item.Name = model.Name;
        item.MPN = model.MPN;
        item.SelectedDeviceTypeId = model.SelectedDeviceTypeId;
        item.SelectedUserId = model.SelectedUserId;

        var response = await deviceClient.CreateDevice(item);
        var productId = response.Data;

        TempData["Issuccessfull"] = response.StatusIsSuccessful;
        TempData["Type"] = "Save";

        return RedirectToAction("Index");
      }

      var deviceTypes = await deviceTypeClient.GetDeviceTypes();
      var lstDeviceTypes = deviceTypes.Data;

      model.ListDeviceTypes = lstDeviceTypes;

      return View(model);
    }

    [AuthenticationAttribute]
    public async Task<ActionResult> UpdateDevice(int id)
    {
      var deviceTypes = await deviceTypeClient.GetDeviceTypes();
      var lstDeviceTypes = deviceTypes.Data;
      var deviceResponse = await deviceClient.GetDevice(id);
      var device = deviceResponse.Data;

      ViewBag.UserRole = tokenContainer.UserRole.ToString();
      ViewBag.Title = "Update Device";

      var model = new CreateDeviceModel()
      {
        Id = device.DeviceId,
        Active = device.Active,
        IP = device.IP,
        MPN = device.MPN,
        Name = device.Name,
        SelectedDeviceTypeId = device.SelectedDeviceTypeId,
        ListDeviceTypes = lstDeviceTypes,
        SelectedUserId = device.SelectedUserId
      };// Create ViewModel via some kind of mapping (check the accompanying VS solution to see this in action)

      if (tokenContainer.UserRole.ToString() != "users")
      {
        var usersResponse = await accountClient.GetUsers();
        model.ListUsers = usersResponse.Data;
      }

      return View("CreateDevice", model);
    }

    [AuthenticationAttribute]
    [HttpPost]
    public async Task<ActionResult> UpdateDevice(CreateDeviceModel model)
    {
      DeviceItem item = new DeviceItem();

      item.DeviceId = model.Id;
      item.Active = model.Active;
      item.IP = model.IP;
      item.MPN = model.MPN;
      item.Name = model.Name;
      item.SelectedDeviceTypeId = model.SelectedDeviceTypeId;
      item.SelectedUserId = model.SelectedUserId;

      var response = await deviceClient.UpdateDevice(item);
      var productId = response.Data;

      TempData["Issuccessfull"] = response.StatusIsSuccessful;
      TempData["Type"] = "Update";

      return RedirectToAction("Index");
    }

    public async Task<ActionResult> DeleteDevice(int id)
    {
      var response = await deviceClient.DeleteDevice(id);

      TempData["Issuccessfull"] = response.StatusIsSuccessful;
      TempData["Type"] = "Delete";

      return RedirectToAction("Index");
    }
  }
}