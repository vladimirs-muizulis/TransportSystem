using Microsoft.AspNetCore.Mvc;
using TransportSystem.Data;
using TransportSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TransportSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransportController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TransportController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transport>>> GetTransports()
        {
            var transports = await _db.Transports.ToListAsync();
            return Ok(transports); // return JSON
        }

        [HttpPost]
        public async Task<IActionResult> AddTransport([FromBody] Transport transport)
        {
            if (transport == null)
                return BadRequest("Bad request!");

            _db.Transports.Add(transport);
            await _db.SaveChangesAsync();
            return Ok(transport); // return JSON
        }

        // Method to search transports by type
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Transport>>> SearchByType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return BadRequest("Type cannot be empty.");
            }

            var transports = await _db.Transports
                .Where(t => t.Type.Contains(type))  // Search by type
                .ToListAsync();

            return Ok(transports);
        }

    }
}