using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vega.Controllers.Resources;
using Vega.Core;
using Vega.Core.Models;

namespace Vega.Controllers
{
    [Route("/api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IMapper mapper;
        private readonly IVehicleRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public VehiclesController(IMapper mapper, IVehicleRepository repository, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }   

        [HttpPost] // /api/vehicles
        [Authorize]
        public async Task<IActionResult> CreateVehicle([FromBody]SaveVehicleResource saveVehicleResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicle = mapper.Map<SaveVehicleResource, Vehicle>(saveVehicleResource);
            vehicle.LastUpdate = DateTime.Now;

            repository.Add(vehicle);
            await unitOfWork.CompleteAsync();

            vehicle = await repository.GeVehicle(vehicle.Id);

            var result = mapper.Map<Vehicle, SaveVehicleResource>(vehicle);
            return Ok(result);
        }

        [HttpPut("{id}")] // /api/vehicles/id
        [Authorize]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] SaveVehicleResource saveVehicleResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicle = await repository.GeVehicle(id);

            if (vehicle == null)
                return NotFound();

            mapper.Map<SaveVehicleResource, Vehicle>(saveVehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;

            await unitOfWork.CompleteAsync();

            vehicle = await repository.GeVehicle(vehicle.Id);
            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await repository.GeVehicle(id, includeRelated: false);

            if (vehicle == null)
                return NotFound();

            repository.Remove(vehicle);
            await unitOfWork.CompleteAsync();

            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await repository.GeVehicle(id);

            if (vehicle == null)
                return NotFound();

            var vehicleResource = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(vehicleResource);
        }

        [HttpGet]
        public async Task<QueryResultResource<VehicleResource>> GetVehicles(VehicleQueryResource vehicleQueryResource)
        {
            var filter = mapper.Map<VehicleQueryResource, VehicleQuery>(vehicleQueryResource);

            var queryResult = await repository.GetVehicles(filter);
            return mapper.Map<QueryResult<Vehicle>, QueryResultResource<VehicleResource>>(queryResult);
        }
    }
}
