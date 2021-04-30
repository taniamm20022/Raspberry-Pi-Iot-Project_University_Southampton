using System;


namespace CSDI.Data.Models
{
  public class EmergencySituationsItem
  {
    public DateTime Date { get; set; }
    public string DeviceName { get; set; }
    public string SensorName { get; set; }
    public string MinValue { get; set; }
    public string MaxValue { get; set; }
    public string Value { get; set; }
    public string UnitName { get; set; }
  }
}