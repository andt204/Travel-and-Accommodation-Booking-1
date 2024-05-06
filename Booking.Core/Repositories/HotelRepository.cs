using BookingHotel.Core.Context;
using BookingHotel.Core.IRepositories;
using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Repositories {
    public class HotelRepository : BaseRepository, IHotelRepository {
        public HotelRepository(BookingHotelDbContext context) : base(context) {
        }

        public async Task<IEnumerable<Hotel>> ListAsync() {
            return await _context.Hotels.ToListAsync();
        }

        public async Task AddAsync(Hotel hotel) {
            await _context.Hotels.AddAsync(hotel);
        }

        public async Task<Hotel> FindByIdAsync(int id) {
            return await _context.Hotels.FindAsync(id);
        }

        public void Update(Hotel hotel) {
            _context.Hotels.Update(hotel);
        }

        //public void Remove(Hotel hotel) {
        //    _context.Hotels.Remove(hotel);
        //}

        public async Task RemoveAsync(int Id) {
            var hotelFind = await _context.Hotels.FindAsync(Id);
            if (hotelFind == null) {
                throw new Exception("Hotel not found");
            }
            _context.Hotels.Remove(hotelFind);
        }

    }
}
