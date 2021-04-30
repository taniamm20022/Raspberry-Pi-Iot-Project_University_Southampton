using System.Collections.Generic;


namespace RPI.Core.Entities
{
    public class DeviceType
    {
        public DeviceType()
        {
            this.IOTDevices = new HashSet<IOTDevice>();
        }

        public string Name { get; set; }

        public int DeviceTypeId { get; set; }
        public ICollection<IOTDevice> IOTDevices { get; set; }
    }
}
