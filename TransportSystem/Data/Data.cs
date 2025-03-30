using Microsoft.EntityFrameworkCore;
using TransportSystem.Models;

namespace TransportSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<TransportSystem.Models.Transport> Transports { get; set; }
        public DbSet<TransportSystem.Models.Route> Routes { get; set; }
    }
}
