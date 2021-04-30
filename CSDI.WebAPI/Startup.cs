  
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using CSDI.Data;
using CSDI.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using RPI.Core;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(LupenM.WebAPI.Startup))]

namespace LupenM.WebAPI
{
  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      GlobalConfiguration.Configure(WebApiConfig.Register);

      app.CreatePerOwinContext(ApplicationDbContext.Create);
      app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

      var oAuthOptions = new OAuthAuthorizationServerOptions
      {
        TokenEndpointPath = new PathString("/api/token"),
        Provider = new ApplicationOAuthProvider(),
        AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
        AllowInsecureHttp = true
      };

      app.UseOAuthAuthorizationServer(oAuthOptions);
      app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

      // app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

      // Enable the application to use bearer tokens to authenticate users
      app.UseOAuthBearerTokens(oAuthOptions);
    }
  }
}