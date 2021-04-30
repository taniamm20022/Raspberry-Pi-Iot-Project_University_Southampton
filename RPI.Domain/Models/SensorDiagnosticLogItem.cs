using System;


namespace CSDI.Data.Models
{
  public class SensorDiagnosticLogItem
  {
    public int SensorDiagnosticId { get; set; }
    public DateTime Date { get; set; }
        public DateTime? IndicationDate { get; set; }
    public string Value { get; set; }
    public string ExpectedValue { get; set; }
    public bool Status { get; set; }
  }
}