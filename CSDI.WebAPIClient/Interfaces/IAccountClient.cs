
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using CSDI.WebAPIClient.DataModels;
using System.Threading.Tasks;

namespace CSDI.WebAPIClient
{
  public interface IAccountClient
  {
    Task<CreateResponse> RegisterUser(RegisterUserItem user);
    Task<CreateResponse> UpdateUser(UsersItem item);
  }
}