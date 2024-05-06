using BookingHotel.Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Context
{
    public interface IBookingHotelDbContext {
        DbSet<Review> Reviews { get; set; }
        DbSet<Hotel> Hotels {  get; set; }
        DbSet<HotelAmentities> HotelAents { get; set; }
        DbSet<Amenity> Amenities {  get; set; }
        DbSet<City> Citys { get; set; }
        DbSet<Room> Rooms { get; set; }
        DbSet<Gallery> Gallerys { get; set; }
        DbSet<RoomClasses> RoomsClasses { get; set;}
        DbSet<Booking> Bookings { get; set; }
        DbSet<Invoice> Invoices { get; set; }
        DbSet<Discount> Discounts { get; set; }
        DbSet<RoomDiscount> RoomDiscounts { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
