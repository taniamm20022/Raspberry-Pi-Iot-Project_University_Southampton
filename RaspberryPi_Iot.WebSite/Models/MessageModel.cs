
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


namespace LupenM.WebSite.Models
{
  public class MessageModel
  {
    public bool IsSuccessfull { get; set; }

    public string Message
    {
      get
      {
        if (this.IsSuccessfull)
        {
          return Type + " Successfull";
        }
        else
        {
          return " Error!" + Type + " has failed";
        }
      }
    }

    public string Type { get; set; }
  }
}