using System;

/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


namespace CSDI.Data.Models
{
  public class IndicationsItem
  {
    public DateTime Date { get; set; }
    public string DeviceName { get; set; }
    public string IP { get; set; }
    public string SensorName { get; set; }
    public string Value { get; set; }
    public string UnitName { get; set; }
    public string MinValue { get; set; }
    public string MaxValue { get; set; }
  }
}