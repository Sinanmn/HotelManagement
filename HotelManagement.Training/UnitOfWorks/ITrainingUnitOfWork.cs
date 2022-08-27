using HotelManagement.Data;
using HotelManagement.Training.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Training.UnitOfWorks
{
    public interface ITrainingUnitOfWork : IUnitOfWork
    {
        IUserRepository Users { get; }
        IRoomRepository Rooms { get; }
        IBookingRepository Bookings { get; }
    }
}
