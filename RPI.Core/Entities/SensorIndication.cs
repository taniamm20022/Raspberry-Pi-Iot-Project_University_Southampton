using System;


namespace RPI.Core.Entities
{
    public class SensorIndication
    {
        public int SensorIndicationId { get; set; }
        public DateTime Date { get; set; }
        public string Value { get; set; }
        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }
    }
}
