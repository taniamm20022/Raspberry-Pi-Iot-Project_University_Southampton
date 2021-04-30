using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Script.Serialization;


namespace CSDI.WebAPIClient
{
    public abstract class ClientBase
    {
        private readonly IApiClient apiClient;

        protected ClientBase(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        protected async Task<TResponse> GetJsonDecodedContent<TResponse, TContentResponse>(
            string uri, 
            params KeyValuePair<string, string>[] requestParameters)
        where TResponse : ApiResponse<TContentResponse>, new()
        {
            var apiResponse = await apiClient.GetFormEncodedContent(uri, requestParameters);
            var response = await CreateJsonResponse<TResponse>(apiResponse);
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = Int32.MaxValue;
            response.Data = js.Deserialize<TContentResponse>(response.ResponseResult);

            return response;
        }

        protected async Task<TResponse> PostEncodedContentWithSimpleResponse<TResponse, TModel>(string url, TModel model)
        where TResponse : ApiResponse<int>, new()
        {
            using (var apiResponse = await apiClient.PostJsonEncodedContent(url, model))
            {
                var response = await CreateJsonResponse<TResponse>(apiResponse);

                response.Data = Json.Decode<int>(response.ResponseResult);

                return response;
            }
        }

        protected async Task<HttpResponseMessage> PostFormEncodedContent(string url, params KeyValuePair<string, string>[] postParameters)
        {
            return await apiClient.PostFormEncodedContent(url, postParameters);
        }

        protected async Task<TResponse> DeleteFormEncodedContentWithSimpleResponse<TResponse>(string url, params KeyValuePair<string, string>[] postParameters)
        where TResponse : ApiResponse<int>, new()
        {
            using (var apiResponse = await apiClient.DeleteFormEncodedContent(url, postParameters))
            {
                var response = await CreateJsonResponse<TResponse>(apiResponse);

                response.Data = Json.Decode<int>(response.ResponseResult);

                return response;
            }
        }

        protected async Task<TResponse> PutEncodedContentWithSimpleResponse<TResponse, TModel>(string url, TModel model)
        //  where TModel : DeviceItem
        where TResponse : ApiResponse<int>, new()
        {
            using (var apiResponse = await apiClient.PutJsonEncodedContent(url, model))
            {
                var response = await CreateJsonResponse<TResponse>(apiResponse);

                response.Data = Json.Decode<int>(response.ResponseResult);

                return response;
            }
        }

        protected static async Task<TResponse> CreateJsonResponse<TResponse>(HttpResponseMessage response) where TResponse : ApiResponse, new()
        {
            var clientResponse = new TResponse
            {
                StatusIsSuccessful = response.IsSuccessStatusCode,
                ResponseCode = response.StatusCode
            };

            if (response.Content != null)
            {
                clientResponse.ResponseResult = await response.Content.ReadAsStringAsync();
            }

            return clientResponse;
        }

        protected static async Task<TContentResponse> DecodeContent<TContentResponse>(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();

            return Json.Decode<TContentResponse>(result);
        }
    }
}