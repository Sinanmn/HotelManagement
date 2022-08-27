using HotelManagement.Data;
using HotelManagement.Training.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Training.Repositories
{
    public interface IBookingRepository : IRepository<Booking,int>
    {
    }
}
