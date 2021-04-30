using System;

namespace CSDI.WebAPI.Models
{
  public class DiagnosticItem
  {
    public DateTime? Date { get; set; }
    public int SensorId { get; set; }
    public int SensorDiagnosticId { get; set; }
    public string SensorName { get; set; }
    public string LastValue { get; set; }
    public TimeSpan Interval { get; set; }
    public bool Status { get; set; }
    public string UnitName { get; set; }
  }
}