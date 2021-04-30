
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using System;

namespace CSDI.WebAPIClient.DataModels
{
  public class SensorItem
  {
    public int SensorId { get; set; }
    public string Name { get; set; }

    public int SelectedDeviceId { get; set; }
    public string DeviceName { get; set; }

    public int SelectdUnitId { get; set; }
    public string UnitName { get; set; }

    public string MinValue { get; set; }
    public string MaxValue { get; set; }

    public TimeSpan IndicationInterval { get; set; }

    public string Topic { get; set; }
    public bool Active { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; }
  }
}