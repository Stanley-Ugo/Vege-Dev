using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vega.Core.Models; 

namespace Vega.Core
{
    public interface IVehicleRepository
    {
        public Task<Vehicle> GeVehicle(int id, bool includeRelated = true);

        public void Add(Vehicle vehicle);
        public void Remove(Vehicle vehicle);

        public Task<QueryResult<Vehicle>> GetVehicles(VehicleQuery queryObj);
    }
}
