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

        // PUT: api/Transport/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransport(int id, [FromBody] Transport transport)
        {
            if (id != transport.Id)
                return BadRequest("ID mismatch");

            var existingTransport = await _db.Transports.FindAsync(id);
            if (existingTransport == null)
                return NotFound();

            existingTransport.Name = transport.Name;
            existingTransport.Type = transport.Type;
            existingTransport.Capacity = transport.Capacity;

            await _db.SaveChangesAsync();
            return Ok(existingTransport);
        }

        // DELETE: api/Transport/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransport(int id)
        {
            var transport = await _db.Transports.FindAsync(id);
            if (transport == null)
                return NotFound();

            _db.Transports.Remove(transport);
            await _db.SaveChangesAsync();
            return NoContent(); // 204 No Content
        }

    }
}