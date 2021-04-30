using System;


namespace RPI.Core.Entities
{
    public class SensorDiagnosticLog
    {
        public int SensorDiagnosticLogId { get; set; }

        public int SensorDiagnosticId { get; set; }

        public DateTime Date { get; set; }

        public DateTime? IndicationDate { get; set; }

        public string Value { get; set; }

        public string ExpectedValue { get; set; }

        public bool Status { get; set; }

        public SensorDiagnostic SensorDiagnostic { get; set; }
    }
}
