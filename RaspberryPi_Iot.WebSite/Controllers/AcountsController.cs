using CSDI.WebAPIClient;
using CSDI.WebAPIClient.DataModels;
using LupenM.WebSite.Attributes;
using LupenM.WebSite.Models;
using LupenM.WebSite.Models.Users;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;



namespace LupenM.WebSite.Controllers
{
  public class AcountsController : Controller
  {
    IHttpClientFactory httpClientFactory;
    TokenContainer tokenContainer;

    private readonly AccountClient accountClient;
    private readonly AccountClient accountClient2;

    public AcountsController()
    {
      this.tokenContainer = new TokenContainer();
      httpClientFactory = new HttpClientFactory();

      var apiClient = new ApiClient(httpClientFactory.CreateClient(), tokenContainer);
      accountClient = new AccountClient(apiClient);

      var apiClient2 = new ApiClient(httpClientFactory.CreateClient(), tokenContainer);
      accountClient2 = new AccountClient(apiClient2);
    }

    public ActionResult Index()
    {
      return View();
    }

    [AuthenticationAttributeUsers]
    public async Task<ActionResult> GetUsers(UserListingPageModel model)
    {
      if (model.PageSize == 0)
      {
        model = new UserListingPageModel();
        model.PageOrder = 1;
        model.CurrentPage = 1;
        model.PageSize = 10;
      }

      var userResponse = await accountClient2.GetUsersList(model.PageSize, model.Position);

      model.Items = userResponse.Data.ListItems;
      model.TotalRecords = userResponse.Data.TotalRecords;
      model.ItemsCount = model.Items.Count();

      return View("Users", model);
    }

    public ActionResult Register()
    {
      var model = new RegisterViewModel();

      return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
      var registeruser = new RegisterUserItem();

      registeruser.Name = model.Name;
      registeruser.Email = model.Email;
      registeruser.Password = model.Password;
      registeruser.ConfirmPassword = model.ConfirmPassword;
      registeruser.Telephone = model.Telephone;

      var response = await accountClient.Register(registeruser);

      if (response.StatusIsSuccessful)
      {
        return RedirectToAction("Index", "Diagnostics");
      }

      //AddResponseErrorsToModelState(response);
      return View(model);
    }

    public ActionResult Login()
    {
      var model = new UserLoginViewModel();

      return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> Login(UserLoginViewModel model)
    {
      var loginSuccess = await PerformLoginActions(model.UserName, model.Password);

      if (loginSuccess)
      {
        return RedirectToAction("Index", "Diagnostics");
      }

      ModelState.Clear();
      ModelState.AddModelError("", "E-mail or password is incorrect.");

      return View(model);
    }

    // Register methods go here, removed for brevity
    private async Task<bool> PerformLoginActions(string email, string password)
    {
      var response = await accountClient.Login(email, password);

      if (response.StatusIsSuccessful)
      {
        tokenContainer.ApiToken = response.Data;
        tokenContainer.UserRole = response.AdditionalInfo;
        tokenContainer.UserName = response.Additional;
      }
      else
      {
        //AddResponseErrorsToModelState(response);
      }

      return response.StatusIsSuccessful;
    }

    public async Task<ActionResult> Logout()
    {
      tokenContainer.ApiToken = null;

      return RedirectToAction("Index", "Device");
    }

    [AuthenticationAttributeUsers]
    public async Task<ActionResult> UserProfile()
    {
      var username = tokenContainer.UserName.ToString();
      var response = await accountClient2.GetUser(username);
      var user = response.Data;

      var model = new RegisterViewModel()
      {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Telephone = user.Telephone,
        Address = user.Address
      };

      if (TempData.ContainsKey("Issuccessfull"))
      {
        model.messageModel = new MessageModel();
        model.messageModel.IsSuccessfull = (bool)TempData["Issuccessfull"];
        model.messageModel.Type = (string)TempData["Type"];
      }

      return View("UserProfile", model);
    }

    [AuthenticationAttributeUsers]
    [HttpPost]
    public async Task<ActionResult> UserProfile(RegisterViewModel model)
    {
      UsersItem item = new UsersItem();
      var username = tokenContainer.UserName.ToString();

      item.Id = model.Id;
      item.Name = model.Name;
      item.Email = model.Email;
      item.Telephone = model.Telephone;
      item.Address = model.Address;

      var response = await accountClient2.UpdateUser(item);
      var user = response.Data;

      TempData["Issuccessfull"] = response.StatusIsSuccessful;
      TempData["Type"] = "Update";

      return RedirectToAction("UserProfile");
    }

    public new RedirectToRouteResult RedirectToAction(string action, string controller)
    {
      return base.RedirectToAction(action, controller);
    }
  }
}