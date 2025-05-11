using Microsoft.EntityFrameworkCore;
using TransportSystem.Models;

namespace TransportSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet для автобусов
        public DbSet<Transport> Transports { get; set; }

        // DbSet для маршрутов автобусов
        public DbSet<BusRoute> BusRoutes { get; set; }

        // DbSet для остановок автобусов
        public DbSet<BusStop> BusStops { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка связи один-ко-многим между маршрутом и остановками
            modelBuilder.Entity<BusStop>()
                .HasOne(bs => bs.BusRoute)
                .WithMany(br => br.Stops)
                .HasForeignKey(bs => bs.BusRouteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Настройка связи один-к-одному между маршрутом и автобусом
            modelBuilder.Entity<BusRoute>()
                .HasOne(br => br.AssignedBus)
                .WithMany()
                .HasForeignKey(br => br.AssignedBusId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
