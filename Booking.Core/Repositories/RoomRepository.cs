using BookingHotel.Core.Context;
using BookingHotel.Core.IRepositories;
using BookingHotel.Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Repositories
{
    public class RoomRepository : BaseRepository, IRoomRepository
    {
        public RoomRepository(BookingHotelDbContext context) : base(context)
        {
        }
        public async Task CreateRoom(Room room)
        {
            await _context.Rooms.AddAsync(room);
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _context.Rooms.ToListAsync();
        }
         
        public async Task<Room> GetByIdAsync(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task RemoveAsync(int Id)
        {
            await Task.Run(() =>
            {
                var roomFind = _context.Rooms.Find(Id);
                if (roomFind == null)
                {
                    throw new Exception("Room not found");
                }
                _context.Rooms.Remove(roomFind);
            });
        }

        public async Task UpdateAsync(Room room)
        {
            await Task.Run(() =>
            {
                _context.Rooms.Update(room);
            });
        }
    }
}
