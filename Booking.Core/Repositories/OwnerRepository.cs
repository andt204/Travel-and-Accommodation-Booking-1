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
    public class OwnerRepository : GenericRepository<User>, IOwnerRepository
    {
        public OwnerRepository(BookingHotelDbContext context) : base(context)
        {
        }
    }
}
