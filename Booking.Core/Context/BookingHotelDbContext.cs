using BookingHotel.Core.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Context
{
    public class BookingHotelDbContext : IdentityDbContext<User>, IBookingHotelDbContext {
        public DbSet<Review> Reviews { get  ; set  ; }
        public DbSet<Hotel> Hotels { get  ; set  ; }
        public DbSet<HotelAmentities> HotelAents { get  ; set  ; }
        public DbSet<Amenity> Amenities { get  ; set  ; }
        public DbSet<City> Cities { get  ; set  ; }
        public DbSet<Room> Rooms { get  ; set  ; }
        public DbSet<Gallery> Gallerys { get  ; set  ; }
        public DbSet<RoomClasses> RoomsClasses { get  ; set  ; }
        public DbSet<Booking> Bookings { get  ; set  ; }
        public DbSet<Invoice> Invoices { get  ; set  ; }
        public DbSet<Discount> Discounts { get  ; set  ; }
        public DbSet<RoomDiscount> RoomDiscounts { get  ; set  ; }
    
        public BookingHotelDbContext(DbContextOptions<BookingHotelDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=BookingHotel;Integrated Security=True;Trust Server Certificate=True; User ID=sa;Password=123;")
        .LogTo(Console.WriteLine, LogLevel.Information);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
        public override int SaveChanges() => base.SaveChanges();

        //Because it have different signatures so it can't overide
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync();
    }
}
