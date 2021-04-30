
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using CSDI.WebAPIClient.DataModels;
using System.Collections.Generic;

namespace CSDI.WebAPIClient
{
  public class UsersResponse : ApiResponse<List<CSDI.WebAPIClient.DataModels.UsersItem>>
  {
  }

  public class UserListResponse : ApiResponse<ListingPageModel<CSDI.WebAPIClient.DataModels.UsersItem>>
  {
  }

  public class UserResponse : ApiResponse<UsersItem>
  {
  }
}