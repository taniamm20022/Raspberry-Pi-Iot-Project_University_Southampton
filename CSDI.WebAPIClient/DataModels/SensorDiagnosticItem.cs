
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using System;

namespace CSDI.WebAPIClient.DataModels
{
  public class SensorDiagnosticItem
  {
    public int SensorDiagnosticId { get; set; }
    public int SensorId { get; set; }
    public DateTime Date { get; set; }
    public bool Status { get; set; }
  }
}
