
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CSDI.WebAPIClient
{
  public class ApiClient : IApiClient
  {
    private const string BaseUri = "http://localhost:58555/";
    //private const string BaseUri = "http://localhost:8080/";

    private readonly System.Net.Http.HttpClient httpClient;
    private readonly TokenContainer tokenContainer;

    public ApiClient(HttpClient httpClient, TokenContainer tokenContainer)
    {
      this.httpClient = httpClient;
      this.tokenContainer = tokenContainer;
    }

    public async Task<HttpResponseMessage> GetFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values)
    {
      httpClient.BaseAddress = new Uri(BaseUri);
      AddToken();

      using (var content = new FormUrlEncodedContent(values))
      {
        var query = await content.ReadAsStringAsync();
        var requestUriWithQuery = string.Concat(requestUri, "?", query);
        var response = await httpClient.GetAsync(requestUriWithQuery);

        return response;
      }
    }

    public async Task<HttpResponseMessage> DeleteFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values)
    {
      httpClient.BaseAddress = new Uri(BaseUri);
      AddToken();

      using (var content = new FormUrlEncodedContent(values))
      {
        var query = await content.ReadAsStringAsync();
        var requestUriWithQuery = string.Concat(requestUri, "?", query);
        var deleteResponse = await httpClient.DeleteAsync(requestUriWithQuery);

        return deleteResponse;
      }
    }

    public async Task<HttpResponseMessage> PutJsonEncodedContent<T>(string requestUri, T content)
    {
      httpClient.BaseAddress = new Uri(BaseUri);
      httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

      AddToken();

      var response = await httpClient.PutAsJsonAsync(requestUri, content);

      return response;
    }

    public async Task<HttpResponseMessage> PostFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values)
    {
      httpClient.BaseAddress = new Uri(BaseUri);

      using (var content = new FormUrlEncodedContent(values))
      {
        var response = await httpClient.PostAsync(requestUri, content);

        return response;
      }
    }

    public async Task<HttpResponseMessage> PostJsonEncodedContent<T>(string requestUri, T content)
    {
      httpClient.BaseAddress = new Uri(BaseUri);
      httpClient.DefaultRequestHeaders.Accept.Clear();
      httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

      AddToken();

      var response = await httpClient.PostAsJsonAsync(requestUri, content);

      return response;
    }

    private void AddToken()
    {
      if (tokenContainer.ApiToken != null)
      {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenContainer.ApiToken.ToString());
      }
    }
  }
}