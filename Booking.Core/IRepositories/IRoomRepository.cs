using BookingHotel.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.IRepositories
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllAsync();
        Task<Room> GetByIdAsync(int id);
        Task CreateRoom(Room room);
        Task UpdateAsync(Room room);
        Task RemoveAsync(int Id);
        Task<bool> CheckRoomStatus(int roomId);
    }
}
