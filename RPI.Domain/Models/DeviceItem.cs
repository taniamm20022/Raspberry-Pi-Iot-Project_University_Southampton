using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LupenM.WebAPI.Models
{
    public class DeviceItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Type { get; set; }
        public string ClientName { get; set; }
        public bool  Active { get; set; }
        public DateTime Date { get; internal set; }
    }
}