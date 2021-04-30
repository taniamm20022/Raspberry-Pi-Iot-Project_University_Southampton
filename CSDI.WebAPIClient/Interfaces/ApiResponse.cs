
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using System.Net;

namespace CSDI.WebAPIClient
{
  public abstract class ApiResponse
  {
    public bool StatusIsSuccessful { get; set; }
    public HttpStatusCode ResponseCode { get; set; }
    public string ResponseResult { get; set; }
    public int TotalRecors { get; set; }
  }

  public abstract class ApiResponse<T> : ApiResponse
  {
    public T Data { get; set; }
    public T AdditionalInfo { get; set; }
    public T Additional { get; set; }
  }
}