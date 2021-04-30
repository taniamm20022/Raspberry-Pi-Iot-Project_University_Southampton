
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using CSDI.WebAPIClient.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSDI.WebAPIClient
{
  public class AccountClient : ClientBase
  {
    private const string RegisterUri = "api/register";
    private const string TokenUri = "api/token";

    public AccountClient(IApiClient apiClient) : base(apiClient)
    {
    }

    public async Task<CreateResponse> Register(RegisterUserItem item)
    {
      var createProductResponse = await PostEncodedContentWithSimpleResponse<CreateResponse, RegisterUserItem>(RegisterUri, item);

      return createProductResponse;
    }

    public async Task<TokenResponse> Login(string email, string password)
    {
      KeyValuePair<string, string>[] keyValuePair = new KeyValuePair<string, string>[]
      {
          new KeyValuePair<string, string>("grant_type", "password"),
          new KeyValuePair<string, string>("username", email),
          new KeyValuePair<string, string>("password", password)
      };

      var response = await PostFormEncodedContent(TokenUri, keyValuePair);
      var tokenResponse = await CreateJsonResponse<TokenResponse>(response);

      if (!response.IsSuccessStatusCode)
      {
        var errorContent = await DecodeContent<dynamic>(response);
        /*tokenResponse.ErrorState = new ErrorStateResponse
        {
            ModelState = new Dictionary<string, string[]>
        {
            {errorContent["error"], new string[] {errorContent["error_description"]}}
        }
        };*/
        return tokenResponse;
      }

      var tokenData = await DecodeContent<dynamic>(response);

      tokenResponse.Data = tokenData["access_token"];
      tokenResponse.AdditionalInfo = tokenData["roles"];
      tokenResponse.Additional = tokenData["userName"];

      return tokenResponse;
    }

    public async Task<UsersResponse> GetUsers()
    {
      var idPair = new KeyValuePair<string, string>();

      return await GetJsonDecodedContent<UsersResponse, List<CSDI.WebAPIClient.DataModels.UsersItem>>(RegisterUri, idPair);
    }

    public async Task<UserResponse> GetUser(string id)
    {
      var idPair = new KeyValuePair<string, string>("id", id);

      return await GetJsonDecodedContent<UserResponse, CSDI.WebAPIClient.DataModels.UsersItem>(RegisterUri, idPair);
    }

    public async Task<UserListResponse> GetUsersList(int pageSize, int position)
    {
      KeyValuePair<string, string>[] keyValuePair = new KeyValuePair<string, string>[]
      {
        new KeyValuePair<string, string>("pageSize", pageSize.ToString()),
        new KeyValuePair<string, string>("position", position.ToString())
      };

      return await GetJsonDecodedContent<UserListResponse, ListingPageModel<CSDI.WebAPIClient.DataModels.UsersItem>>(RegisterUri, keyValuePair);
    }

    public async Task<CreateResponse> UpdateUser(UsersItem userItem)
    {
      var updateUserResponse = await PutEncodedContentWithSimpleResponse<CreateResponse, UsersItem>(RegisterUri, userItem);

      return updateUserResponse;
    }
  }
}