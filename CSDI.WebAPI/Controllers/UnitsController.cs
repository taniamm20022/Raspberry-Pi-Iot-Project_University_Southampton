using CSDI.Data;
using CSDI.Data.Services.Interfaces;
using CSDI.WebAPI.Models;
using RPI.Core.Entities;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;


namespace LupenM.WebAPI.Controllers
{
    public class UnitsController : ApiController
    {
        private readonly IUnitsService _unitsService;

        public UnitsController(IUnitsService unitsService)
        {
            _unitsService = unitsService;
        }

        public ListingPageModel<UnitItem> Get(int position, int pageSize)
        {
            ListingPageModel<UnitItem> result = new ListingPageModel<UnitItem>();
            List<UnitItem> lstUnits = new List<UnitItem>();

            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            bool isAdmin = principal.IsInRole("admins");

            List<Unit> units = new List<Unit>();

            int totalRecords;

            units = UnitsRepository.GetAll(position, pageSize, out totalRecords);

            foreach (var unit in units)
            {
                lstUnits.Add(new UnitItem
                {
                    Id = unit.UnitId,
                    Name = unit.Name
                });
            }

            result.ListItems = lstUnits;
            result.TotalRecords = totalRecords;

            return result;
        }

        public List<UnitItem> Get()
        {
            List<UnitItem> lstUnits = new List<UnitItem>();

            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            bool isAdmin = principal.IsInRole("admins");

            List<Unit> units = new List<Unit>();
            units = UnitsRepository.GetAll();

            foreach (var unit in units)
            {
                lstUnits.Add(new UnitItem
                {
                    Id = unit.UnitId,
                    Name = unit.Name
                });
            }

            return lstUnits;
        }

        public IHttpActionResult Get(int id)
        {
            var unit = _unitsService.GetSingle(
                x => x.UnitId == id);

            if (unit == null)
            {
                return NotFound();
            }

            var result = new TypeItem()
            {
                Id = unit.UnitId,
                Name = unit.Name
            };

            return Ok(result);
        }

        public IHttpActionResult Post(UnitItem unitItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var unit = new Unit
            {
                Name = unitItem.Name
            };

            _unitsService.Add(unit);
            return Ok(unit.UnitId);
        }

        public IHttpActionResult Put(UnitItem unitItem)
        {
            var unit = _unitsService.GetSingle(
                x => x.UnitId == unitItem.Id);

            unit.Name = unitItem.Name;
            _unitsService.Update(unit);

            return Ok(unit.UnitId);
        }

        public IHttpActionResult Delete(int id)
        {
            var unit = _unitsService.GetSingle(
                x=>x.UnitId==id);

            if (unit == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var result = _unitsService.Delete(unit);

            return Ok(result);
        }
    }
}