using CSDI.Data;
using CSDI.Data.Repositories;
using CSDI.Data.Services.Interfaces;
using CSDI.WebAPI.Models;
using RPI.Core.Entities;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;


namespace LupenM.WebAPI.Controllers
{
    public class DeviceTypesController : ApiController
    {
        private readonly IDeviceTypesService _deviceTypesService;

        public DeviceTypesController(IDeviceTypesService deviceTypesService)
        {
            _deviceTypesService = deviceTypesService;
        }
        public ListingPageModel<TypeItem> Get(int position, int pageSize)
        {
            ListingPageModel<TypeItem> result = new ListingPageModel<TypeItem>();
            List<TypeItem> lstDeviceTypes = new List<TypeItem>();

            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            bool isAdmin = principal.IsInRole("admins");

            List<DeviceType> deviceTypes = new List<DeviceType>();

            int totalRecords;

            deviceTypes = DeviceTypesRepository.GetAll(position, pageSize, out totalRecords);

            foreach (var deviceType in deviceTypes)
            {
                lstDeviceTypes.Add(new TypeItem
                {
                    Id = deviceType.DeviceTypeId,
                    Name = deviceType.Name
                });
            }

            result.ListItems = lstDeviceTypes;
            result.TotalRecords = totalRecords;

            return result;
        }

        public List<TypeItem> Get()
        {
            List<TypeItem> lstDeviceTypes = new List<TypeItem>();

            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            bool isAdmin = principal.IsInRole("admins");

            List<DeviceType> deviceTypes = new List<DeviceType>();
            deviceTypes = DeviceTypesData.GetAll();

            foreach (var deviceType in deviceTypes)
            {
                lstDeviceTypes.Add(new TypeItem
                {
                    Id = deviceType.DeviceTypeId,
                    Name = deviceType.Name
                });
            }

            return lstDeviceTypes;
        }

        public IHttpActionResult Get(int id)
        {
            var deviceType = _deviceTypesService.GetSingle(
                x => x.DeviceTypeId == id);

            if (deviceType == null)
            {
                return NotFound();
            }

            var result = new TypeItem()
            {
                Id = deviceType.DeviceTypeId,
                Name = deviceType.Name
            };

            return Ok(result);
        }

        public IHttpActionResult Post(TypeItem deviceTypeItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deviceType = new DeviceType
            {
                Name = deviceTypeItem.Name
            };

            _deviceTypesService.Add(deviceType);

            return Ok(deviceType.DeviceTypeId);
        }

        public IHttpActionResult Put(TypeItem deviceTypeItem)
        {
            var deviceType = _deviceTypesService.GetSingle(
                x => x.DeviceTypeId == deviceTypeItem.Id);
            deviceType.Name = deviceTypeItem.Name;

            _deviceTypesService.Update(deviceType);

            return Ok();
        }

        public IHttpActionResult Delete(int id)
        {
            var deviceType = _deviceTypesService.GetSingle(
                x => x.DeviceTypeId == id);

            if (deviceType == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var result = _deviceTypesService.Delete(
                deviceType);

            return Ok(result);
        }
    }
}