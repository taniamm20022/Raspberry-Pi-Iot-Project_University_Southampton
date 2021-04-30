using CSDI.WebAPIClient;
using CSDI.WebAPIClient.DataModels;
using LupenM.WebSite.Attributes;
using LupenM.WebSite.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;



namespace LupenM.WebSite.Controllers
{
  public class UnitsController : Controller
  {
    IHttpClientFactory httpClientFactory;
    private readonly TokenContainer tokenContainer;

    private readonly UnitClient unitsClient;

    public UnitsController()
    {
      httpClientFactory = new HttpClientFactory();
      this.tokenContainer = new TokenContainer();

      var apiClient = new ApiClient(httpClientFactory.CreateClient(), tokenContainer);
      unitsClient = new UnitClient(apiClient);
    }

    [AuthencticationAttributeUnits]
    public async Task<ActionResult> Index(UnitsListingModel model)
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

      var units = await unitsClient.GetUnits(model.SelectedUserId, model.PageSize, model.Position);

      model.Items = units.Data.ListItems;
      model.TotalRecords = units.Data.TotalRecords;
      model.ItemsCount = model.Items.Count();

      return View(model);
    }

    [AuthencticationAttributeUnits]
    public ActionResult CreateUnit()
    {
      var model = new CreateUnitModel();

      ViewBag.UserRole = tokenContainer.UserRole.ToString();
      ViewBag.Title = "Register Unit";

      return View(model);
    }

    [AuthencticationAttributeUnits]
    [HttpPost]
    public async Task<ActionResult> CreateUnit(CreateUnitModel model)
    {
      if (ModelState.IsValid)
      {
        UnitItem item = new UnitItem();
        item.Name = model.Name;

        var response = await unitsClient.CreateUnit(item);
        var productId = response.Data;

        TempData["Issuccessfull"] = response.StatusIsSuccessful;
        TempData["Type"] = "Save";

        return RedirectToAction("Index");
      }

      return View(model);
    }

    [AuthencticationAttributeUnits]
    public async Task<ActionResult> UpdateUnit(int id)
    {
      var unitResponse = await unitsClient.GetUnit(id);
      var unit = unitResponse.Data;

      ViewBag.Title = "Update Unit";

      var model = new CreateUnitModel()
      {
        Id = unit.Id,
        Name = unit.Name
      };

      return View("CreateUnit", model);
    }

    [AuthencticationAttributeUnits]
    [HttpPost]
    public async Task<ActionResult> UpdateUnit(CreateUnitModel model)
    {
      UnitItem item = new UnitItem();

      item.Id = model.Id;
      item.Name = model.Name;

      var response = await unitsClient.UpdateUnit(item);
      var productId = response.Data;

      TempData["Issuccessfull"] = response.StatusIsSuccessful;
      TempData["Type"] = "Update";

      return RedirectToAction("Index");
    }

    [AuthencticationAttributeUnits]
    public async Task<ActionResult> DeleteUnit(int id)
    {
      var response = await unitsClient.DeleteUnit(id);

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