
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


namespace CSDI.WebAPIClient.DataModels
{
  public class DeviceItem
  {
    public int DeviceId { get; set; }
    public System.DateTime Date { get; set; }
    public string IP { get; set; }
    public string Name { get; set; }
    public string MPN { get; set; }
    public string SelectedUserId { get; set; }
    public int SelectedDeviceTypeId { get; set; }
    public bool Active { get; set; }
    public string TypeName { get; set; }
    public string UserName { get; set; }
  }
}