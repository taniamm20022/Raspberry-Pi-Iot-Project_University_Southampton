using CSDI.Data.Services.Interfaces;
using CSDI.WebAPI.Models;
using Microsoft.AspNet.Identity;
using RPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using tesr.Data;

namespace LupenM.WebAPI.Controllers
{
    public class SensorsController : ApiController
    {
        #region Propeties 
        private readonly ISensorsService _sensorsService;
        #endregion 


        public SensorsController(ISensorsService sensorsService)
        {
            _sensorsService = sensorsService;
        }

        public ListingPageModel<SensorItem> Get(PageFilter pageFilter)
        {
            ListingPageModel<SensorItem> result = new ListingPageModel<SensorItem>();
            List<SensorItem> lstSensors = new List<SensorItem>();

            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            bool isAdmin = principal.IsInRole("admins");

            var userId = User.Identity.GetUserId();
            IEnumerable<Sensor> sensors;

            int totalRecords;

            if (isAdmin)
            {
                sensors = _sensorsService.GetPagedListAsync(
                    pageFilter.PagingRequest,
                    )
                    getGetAll(position, pageSize, out totalRecords);
            }
            else
            {
                sensors = SensorsData.GetAllByPerson(userId, position, pageSize, out totalRecords);
            }

            foreach (var sensor in sensors)
            {
                lstSensors.Add(new SensorItem
                {
                    SensorId = sensor.SensorId,
                    SelectedDeviceId = sensor.DeviceId,
                    SelectdUnitId = sensor.UnitId,
                    DeviceName = sensor.Device.Name,
                    UnitName = sensor.Unit.Name,
                    Name = sensor.Name,
                    Location = sensor.Location
                });
            }

            result.ListItems = lstSensors;
            result.TotalRecords = totalRecords;

            return result;
        }

        public IHttpActionResult Get(int id)
        {
            var sensor = _sensorsService.GetSingle(
                x => x.DeviceId == id);

            if (sensor == null)
            {
                return NotFound();
            }

            var result = new SensorItem()
            {
                SensorId = sensor.SensorId,
                Name = sensor.Name,
                Date = sensor.Date,
                MinValue = sensor.MinValue,
                MaxValue = sensor.MaxValue,
                IndicationInterval = sensor.IndicationInterval,
                Topic = sensor.Topic,
                SelectedDeviceId = sensor.DeviceId,
                SelectdUnitId = sensor.UnitId,
                Active = sensor.Active,
                Location = sensor.Location,
                UnitName = sensor.Unit.Name
            };

            return Ok(result);
        }

        public IHttpActionResult Post(SensorItem sensor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _sensorsService.Add(new Sensor
            {
                Name = sensor.Name,
                Active = sensor.Active,
                DeviceId = sensor.SelectedDeviceId,
                UnitId = sensor.SelectdUnitId,
                Date = DateTime.Now,
                MinValue = sensor.MinValue,
                MaxValue = sensor.MaxValue,
                IndicationInterval = sensor.IndicationInterval,
                Topic = sensor.Topic,
                Location = sensor.Location
            });

            return CreatedAtRoute("DefaultApi", new { Id = result }, sensor);
        }

        public IHttpActionResult Put(SensorItem sensorItem)
        {
            var sensor = _sensorsService.GetSingle(
               x => x.SensorId == sensorItem.SensorId);

            sensor.Name = sensorItem.Name;
            sensor.DeviceId = sensorItem.SelectedDeviceId;
            sensor.MaxValue = sensorItem.MaxValue;
            sensor.MinValue = sensorItem.MinValue;
            sensor.Topic = sensorItem.Topic;
            sensor.IndicationInterval = sensorItem.IndicationInterval;
            sensor.UnitId = sensorItem.SelectdUnitId;
            sensor.Active = sensorItem.Active;
            sensor.Location = sensorItem.Location;

            _sensorsService.Update(sensor);

            return Ok(result);
        }

        public IHttpActionResult Delete(int id)
        {
            var sensor = _sensorsService.GetSingle(
                x => x.SensorId == id);

            if (sensor == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var result = _sensorsService.Delete(
                sensor);

            return Ok(result);
        }
    }
}