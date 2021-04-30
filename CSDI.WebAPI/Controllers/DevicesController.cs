using CSDI.Identity;
using CSDI.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using RPI.Core.Entities;
using CSDI.Data.Repositories;
using CSDI.Data.Services.Interfaces;


namespace LupenM.WebAPI.Controllers
{
    public class DevicesController : ApiController
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        private IDevicesService _devicesService;

        public DevicesController(IDevicesService devicesService)
        {
            _devicesService = devicesService;
        }

        [Authorize]
        public List<DeviceItem> Get()
        {
            List<DeviceItem> result = new List<DeviceItem>();

            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            bool isAdmin = principal.IsInRole("admins");

            var userId = User.Identity.GetUserId();
            IEnumerable<IOTDevice> devices;

            if (isAdmin)
            {
                devices = DevicesRepository.GetAll();
            }
            else
            {
                devices = DevicesRepository.GetAllDevicesForUser(userId);
            }

            var jsonSerialiser = new JavaScriptSerializer();

            foreach (var device in devices)
            {
                result.Add(new DeviceItem
                {
                    DeviceId = device.DeviceId,
                    UserName = device.User.Email,
                    IP = device.IP,
                    MPN = device.MPN,
                    Name = device.Name,
                    SelectedDeviceTypeId = device.DeviceType.DeviceTypeId,
                    Active = device.Active
                });
            }

            return result;
        }

        public ListingPageModel<DeviceItem> Get(string userIdFilter, int position, int pageSize)
        {
            ListingPageModel<DeviceItem> result = new ListingPageModel<DeviceItem>();
            List<DeviceItem> lstDevices = new List<DeviceItem>();

            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            bool isAdmin = principal.IsInRole("admins");

            var userId = User.Identity.GetUserId();
            int totalRecords;

            IEnumerable<IOTDevice> devices;

            if (isAdmin)
            {
                devices = _devicesService.get(userIdFilter, position, pageSize, out totalRecords);
            }
            else
            {
                devices = DevicesRepository.GetAllDevicesForUser(userId, position, pageSize, out totalRecords);
            }

            var jsonSerialiser = new JavaScriptSerializer();

            foreach (var device in devices)
            {
                lstDevices.Add(new DeviceItem
                {
                    DeviceId = device.DeviceId,
                    UserName = device.User.Email,
                    IP = device.IP,
                    MPN = device.MPN,
                    Name = device.Name,
                    SelectedDeviceTypeId = device.DeviceType.DeviceTypeId,
                    Active = device.Active
                });
            }

            result.ListItems = lstDevices;
            result.TotalRecords = totalRecords;

            return result;
        }

        public IHttpActionResult Get(int id)
        {
            var device = _devicesService.GetSingle(
                x => x.DeviceId == id);

            if (device == null)
            {
                return NotFound();
            }

            var result = new DeviceItem()
            {
                DeviceId = device.DeviceId,
                Name = device.Name,
                Date = device.Date,
                IP = device.IP,
                MPN = device.MPN,
                UserName = device.User.Email,
                SelectedUserId = device.User.Id,
                SelectedDeviceTypeId = device.DeviceTypeId,
                Active = device.Active
            };

            return Ok(result);
        }

        [Authorize]
        [ResponseType(typeof(DeviceItem))]
        public IHttpActionResult Post(DeviceItem deviceItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user;
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            bool isAdmin = principal.IsInRole("admins");

            if (isAdmin && deviceItem.SelectedUserId != null)
            {
                user = DevicesRepository.GetCurrentUser(deviceItem.SelectedUserId);
            }
            else
            {
                var userId = User.Identity.GetUserId();
                user = DevicesRepository.GetCurrentUser(userId);
            }
            var device = new IOTDevice
            {
                Name = deviceItem.Name,
                Active = deviceItem.Active,
                IP = deviceItem.IP,
                MPN = deviceItem.MPN,
                DeviceTypeId = deviceItem.SelectedDeviceTypeId,
                Date = DateTime.Now
            };

            _devicesService.Add(device);

            return Ok(device.DeviceId);
        }

        public IHttpActionResult Put(DeviceItem deviceItem)
        {
            var device = _devicesService.GetSingle(
              x => x.DeviceId == deviceItem.DeviceId);

            device.Name = deviceItem.Name;
            device.IP = deviceItem.IP;
            device.MPN = deviceItem.MPN;
            device.DeviceTypeId = deviceItem.SelectedDeviceTypeId;
            device.Active = deviceItem.Active;

            return Ok(device);
        }

        public IHttpActionResult Delete(int id)
        {
            var device = _devicesService.GetSingle(
                x => x.DeviceId == id);

            if (device == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var result = _devicesService.Delete(device);

            return Ok(result);
        }
    }
}