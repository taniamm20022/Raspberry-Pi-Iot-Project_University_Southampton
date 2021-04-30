
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using CSDI.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LupenM.WebAPI
{
  internal class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
  {
    public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    {
      context.Validated();

      return Task.FromResult<object>(null);
    }

    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {
      var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
      var user = await userManager.FindAsync(context.UserName, context.Password);
      if (user == null)
      {
        context.SetError("invalid_grant", "The user name or password is incorrect.");
        return;
      }

      var oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
      var cookiesIdentity = await user.GenerateUserIdentityAsync(userManager, Microsoft.Owin.Security.Cookies.CookieAuthenticationDefaults.AuthenticationType);
      var userRoles = await userManager.GetRolesAsync(user.Id.ToString());
      string joined = string.Join(",", userRoles);
      var properties = CreateProperties(user.UserName, joined);
      var role = user.Roles;




      ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);
      identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
      identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
      identity.AddClaim(new Claim(ClaimTypes.Role, "users"));
      var ticket = new AuthenticationTicket(oAuthIdentity, properties);
      context.Validated(ticket);
      context.Request.Context.Authentication.SignIn(cookiesIdentity);
    }

    public override Task TokenEndpoint(OAuthTokenEndpointContext context)
    {
      foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
      {
        context.AdditionalResponseParameters.Add(property.Key, property.Value);
      }

      return Task.FromResult<object>(null);
    }
    private static AuthenticationProperties CreateProperties(string userName, string userroles)
    {
      var data = new Dictionary<string, string>
        {
            { "userName", userName },

                { "roles", userroles}
        };
      return new AuthenticationProperties(data);
    }
  }
}