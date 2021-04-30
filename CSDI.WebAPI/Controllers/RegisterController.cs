using CSDI.Identity;
using CSDI.WebAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RPI.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;


namespace LupenM.WebAPI.Controllers
{
  public class RegisterController : ApiController
  {
    private ApplicationUserManager UserManager
    {
      get
      {
        return Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
      }
    }

    public RegisterController()
    {
    }

    public List<UserItem> Get()
    {
      List<UserItem> result = new List<UserItem>();

      var users = UserManager.Users;

      foreach (var user in users)
      {
        result.Add(new UserItem { Id = user.Id, UserName = user.UserName, Name = user.Name, TelePhone = user.Telephone, Address = user.Address });
      }

      return result;
    }

    public IHttpActionResult Get(string id)
    {
      var userid = User.Identity.GetUserId();
      var user = UserManager.Users.Where(x => x.UserName == id).SingleOrDefault();

      if (user != null && user.Id == userid)
      {
        var result = new UserItem()
        {
          Id = user.Id,
          Name = user.Name,
          Email = user.Email,
          TelePhone = user.Telephone,
          Address = user.Address
        };

        return Ok(result);
      }
      else
      {
        return NotFound();
      }
    }

    public ListingPageModel<UserItem> Get(int pageSize, int position)
    {
      ListingPageModel<UserItem> result = new ListingPageModel<UserItem>();
      List<UserItem> lstUsers = new List<UserItem>();

      var users = UserManager.Users.OrderBy(x => x.Id).Skip(position).Take(pageSize);

      foreach (var user in users)
      {
        lstUsers.Add(new UserItem { Id = user.Id, UserName = user.UserName, Name = user.Name, TelePhone = user.Telephone, Address = user.Address });
      }

      result.ListItems = lstUsers;
      result.TotalRecords = UserManager.Users.Count();

      return result;
    }

    public IHttpActionResult Post(RegisterApiModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var user = new User
      {
        Name = model.Name,
        Email = model.Email,
        UserName = model.Email,
        EmailConfirmed = true,
        Telephone = model.Telephone
      };

      var result = UserManager.Create(user, model.Password);
      var result2 = UserManager.AddToRole(user.Id, "users");

      return result.Succeeded ? Ok(1) : GetErrorResult(result);
    }

    [Authorize]
    public IHttpActionResult Put(UserItem userItem)
    {
      var userId = User.Identity.GetUserId();
      var user = UserManager.Users.Where(x => x.Id == userId).SingleOrDefault();

      user.Name = userItem.Name;
      user.Telephone = userItem.TelePhone;
      user.Address = userItem.Address;

      var resultUpdate = UserManager.Update(user);

      return resultUpdate.Succeeded ? Ok(1) : GetErrorResult(resultUpdate);
    }

    private IHttpActionResult GetErrorResult(IdentityResult result)
    {
      if (result == null)
      {
        return InternalServerError();
      }

      if (result.Errors != null)
      {
        foreach (var error in result.Errors)
        {
          ModelState.AddModelError("", error);
        }
      }

      if (ModelState.IsValid)
      {
        // No ModelState errors are available to send, so just return an empty BadRequest.
        return BadRequest();
      }

      return BadRequest(ModelState);
    }
  }
}