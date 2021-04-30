using LupenM.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LupenM.WebSite.Models
{
    public class DeviceListViewModel
    {

        IEnumerable<DeviceItem> ListDevices { get; set; }
        IEnumerable<UserItem> ListUsers { get; set; }
    }
}