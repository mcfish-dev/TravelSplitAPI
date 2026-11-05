using Microsoft.EntityFrameworkCore;
using TravelSplitAPI.Models;

namespace TravelSplitAPI.Data
{
    public class TripSqlite : DbContext
    {
        public TripSqlite(DbContextOptions<TripSqlite> options)
            : base(options)
        {
        }

        public DbSet<Trip> Trips => Set<Trip>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Trip>().HasData(TripStore.Trips);
        }
    }
}