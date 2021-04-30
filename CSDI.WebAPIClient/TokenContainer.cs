
using System.Web;

namespace CSDI.WebAPIClient
{
  public class TokenContainer
  {
    private const string ApiTokenKey = "ApiToken";
    private const string UserRoleKey = "UserRole";
    private const string UserNameKey = "UserName";

    public object ApiToken
    {
      get { return Current.Session != null ? Current.Session[ApiTokenKey] : null; }
      set { if (Current.Session != null) Current.Session[ApiTokenKey] = value; }
    }

    public object UserRole
    {
      get { return Current.Session != null ? Current.Session[UserRoleKey] : null; }
      set { if (Current.Session != null) Current.Session[UserRoleKey] = value; }
    }

    private static HttpContextBase Current
    {
      get { return new HttpContextWrapper(HttpContext.Current); }
    }

    public object UserName
    {
      get { return Current.Session != null ? Current.Session[UserNameKey] : null; }
      set { if (Current.Session != null) Current.Session[UserNameKey] = value; }
    }
  }
}