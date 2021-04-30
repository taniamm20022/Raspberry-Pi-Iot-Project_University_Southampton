
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CSDI.WebAPIClient
{
  public interface IApiClient
  {
    Task<HttpResponseMessage> GetFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values);
    Task<HttpResponseMessage> PostJsonEncodedContent<T>(string requestUri, T content);
    Task<HttpResponseMessage> PutJsonEncodedContent<T>(string requestUri, T content);

    Task<HttpResponseMessage> PostFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values);
    Task<HttpResponseMessage> DeleteFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values);
  }
}