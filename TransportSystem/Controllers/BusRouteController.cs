using Microsoft.AspNetCore.Mvc;
using TransportSystem.Data;
using TransportSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace TransportSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusRouteController : ControllerBase
    {
        private readonly AppDbContext _db;

        public BusRouteController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/BusRoute
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetBusRoutes()
        {
            var routes = await _db.BusRoutes
                .Include(r => r.Stops.OrderBy(s => s.Order))
                .Include(r => r.AssignedBus)
                .ToListAsync();

            // type suitable for JSON response
            var result = routes.Select(r => new
            {
                r.Id,
                r.Name,
                r.AssignedBusId,
                AssignedBusName = r.AssignedBus?.Name,
                Stops = r.Stops.Select(s => new
                {
                    s.Id,
                    s.Location,
                    Arrival = s.ArrivalTime.ToString(@"hh\:mm"),  // Format TimeSpan as hh:mm
                    Departure = s.DepartureTime.ToString(@"hh\:mm"),
                    s.Order
                }).ToList()
            });

            return Ok(result);
        }

        // POST: api/BusRoute
        [HttpPost]
        public async Task<IActionResult> AddBusRoute([FromBody] BusRoute route)
        {
            if (route == null || route.Stops == null || !route.Stops.Any())
                return BadRequest("Route and stops are required.");

            foreach (var stop in route.Stops)
            {
                stop.BusRoute = null;
            }

            _db.BusRoutes.Add(route);
            await _db.SaveChangesAsync();

            return Ok(route);
        }

        // PUT: api/BusRoute/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBusRoute(int id, [FromBody] BusRoute route)
        {
            if (id != route.Id)
                return BadRequest("ID mismatch");

            var existingRoute = await _db.BusRoutes
                .Include(r => r.Stops)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (existingRoute == null)
                return NotFound();

            // Remove all existing stops to fully replace them with new ones
            _db.BusStops.RemoveRange(existingRoute.Stops);

            // Update basic route info
            existingRoute.Name = route.Name;
            existingRoute.AssignedBusId = route.AssignedBusId;

            // Reassign new stops
            existingRoute.Stops = route.Stops.Select(s => new BusStop
            {
                Location = s.Location,
                ArrivalTime = s.ArrivalTime,
                DepartureTime = s.DepartureTime,
                Order = s.Order
            }).ToList();

            await _db.SaveChangesAsync();
            return Ok(existingRoute);
        }

        // DELETE: api/BusRoute/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusRoute(int id)
        {
            var route = await _db.BusRoutes
                .Include(r => r.Stops)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (route == null)
                return NotFound();

            // Clean up child entities (stops)
            _db.BusStops.RemoveRange(route.Stops);
            _db.BusRoutes.Remove(route);

            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
