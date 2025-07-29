using ASP.NETCoreRestAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCoreRestAPI.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Details = "A luxurious villa with a private pool and garden.",
                    ImageUrl = "https://example.com/images/royal-villa.jpg",
                    Occupancy = 4,
                    Rate = 200.00,
                    Sqft = 1500,
                    Amenity = "",
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now)
                },
                new Villa()
                {
                    Id = 2,
                    Name = "Beachfront Villa",
                    Details = "A beautiful villa located right on the beach.",
                    ImageUrl = "https://example.com/images/beachfront-villa.jpg",
                    Occupancy = 6,
                    Rate = 300.00,
                    Sqft = 1800,
                    Amenity = "",
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now)
                },
                new Villa()
                {
                    Id = 3,
                    Name = "Mountain View Villa",
                    Details = "A serene villa with stunning mountain views.",
                    ImageUrl = "https://example.com/images/mountain-view-villa.jpg",
                    Occupancy = 5,
                    Rate = 250.00,
                    Sqft = 1600,
                    Amenity = "",
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now)
                },
                new Villa()
                {
                    Id = 4,
                    Name = "Garden Villa",
                    Details = "A charming villa surrounded by lush gardens.",
                    ImageUrl = "https://example.com/images/garden-villa.jpg",
                    Occupancy = 3,
                    Rate = 180.00,
                    Sqft = 1400,
                    Amenity = "",
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now)
                },
                new Villa()
                {
                    Id = 5,
                    Name = "City Center Villa",
                    Details = "A modern villa located in the heart of the city.",
                    ImageUrl = "https://example.com/images/city-center-villa.jpg",
                    Occupancy = 2,
                    Rate = 220.00,
                    Sqft = 1300,
                    Amenity = "",
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now)
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }


}
