
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using System;

namespace CSDI.WebAPIClient.DataModels
{
  public class DiagnosticItem
  {
    public DateTime? Date { get; set; }
    public string SensorId { get; set; }
    public int SensorDiagnosticId { get; set; }
    public string SensorName { get; set; }
    public string LastValue { get; set; }
    public TimeSpan Interval { get; set; }
    public bool Status { get; set; }
    public string UnitName { get; set; }
  }
}