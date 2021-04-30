using CSDI.WebAPIClient;
using CSDI.WebAPIClient.DataModels;
using LupenM.WebSite.Attributes;
using LupenM.WebSite.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace LupenM.WebSite.Controllers
{
  public class DeviceTypesController : Controller
  {
    IHttpClientFactory httpClientFactory;
    private readonly TokenContainer tokenContainer;

    private readonly DeviceTypesClient deviceTypesClient;

    public DeviceTypesController()
    {
      httpClientFactory = new HttpClientFactory();
      this.tokenContainer = new TokenContainer();

      var apiClient = new ApiClient(httpClientFactory.CreateClient(), tokenContainer);
      deviceTypesClient = new DeviceTypesClient(apiClient);
    }

    [AuthenticationAttributeDeviceTypes]
    public async Task<ActionResult> Index(DeviceTypesListingModel model)
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

      var devices = await deviceTypesClient.GetDeviceTypess(model.SelectedUserId, model.PageSize, model.Position);

      model.Items = devices.Data.ListItems;
      model.TotalRecords = devices.Data.TotalRecords;
      model.ItemsCount = model.Items.Count();

      return View(model);
    }

    [AuthenticationAttributeDeviceTypes]
    public ActionResult CreateDeviceType()
    {
      var model = new CreateTypeModel();

      ViewBag.UserRole = tokenContainer.UserRole.ToString();
      ViewBag.Title = "Register Device Type";

      return View(model);
    }

    [AuthenticationAttributeDeviceTypes]
    [HttpPost]
    public async Task<ActionResult> CreateDeviceType(CreateTypeModel model)
    {
      if (ModelState.IsValid)
      {
        TypeItem item = new TypeItem();

        item.Name = model.Name;

        var response = await deviceTypesClient.CreateDeviceType(item);
        var productId = response.Data;

        TempData["Issuccessfull"] = response.StatusIsSuccessful;
        TempData["Type"] = "Save";

        return RedirectToAction("Index");
      }
      return View(model);
    }

    [AuthenticationAttributeDeviceTypes]
    public async Task<ActionResult> UpdateDeviceType(int id)
    {
      var deviceResponse = await deviceTypesClient.GetDeviceType(id);
      var deviceType = deviceResponse.Data;

      ViewBag.Title = "Update Device Type";

      var model = new CreateTypeModel() { Id = deviceType.Id, Name = deviceType.Name };// Create ViewModel via some kind of mapping (check the accompanying VS solution to see this in action)        

      return View("CreateDeviceType", model);
    }

    [AuthenticationAttributeDeviceTypes]
    [HttpPost]
    public async Task<ActionResult> UpdateDeviceType(CreateTypeModel model)
    {
      TypeItem item = new TypeItem();

      item.Id = model.Id;
      item.Name = model.Name;

      var response = await deviceTypesClient.UpdateDeviceType(item);
      var productId = response.Data;

      TempData["Issuccessfull"] = response.StatusIsSuccessful;
      TempData["Type"] = "Update";

      return RedirectToAction("Index");
    }

    [AuthenticationAttributeDeviceTypes]
    public async Task<ActionResult> DeleteDeviceType(int id)
    {
      var response = await deviceTypesClient.DeleteDeviceType(id);

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