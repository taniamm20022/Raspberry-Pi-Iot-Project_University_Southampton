using System;
using System.ComponentModel.DataAnnotations;


namespace RPI.Core.Entities
{
    public class Sensor
    {
        public int SensorId { get; set; }

        public int DeviceId { get; set; }

        public System.DateTime Date { get; set; }

        [Required]
        public string Name { get; set; }

        public int UnitId { get; set; }

        public bool Active { get; set; }

        public String MinValue { get; set; }

        public String MaxValue { get; set; }

        [Required]
        public String Topic { get; set; }

        [Required]
        public TimeSpan IndicationInterval { get; set; }

        public IOTDevice Device { get; set; }

        public Unit Unit { get; set; }

        public string Location { get; set; }
    }
}

