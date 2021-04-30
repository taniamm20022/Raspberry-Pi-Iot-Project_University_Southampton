using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace RPI.Core.Entities
{
    public class IOTDevice
    {

        public IOTDevice()
        {
            this.Sensors = new HashSet<Sensor>();
        }

        public int DeviceId { get; set; }

        public System.DateTime Date { get; set; }

        [Required]
        public string IP { get; set; }

        [Required]
        public string Name { get; set; }

        public string MPN { get; set; }

        public bool Active { get; set; }

        public int DeviceTypeId { get; set; }
        public virtual DeviceType DeviceType { get; set; }

        public User User { get; set; }

        public ICollection<Sensor> Sensors { get; set; }
    }
}
