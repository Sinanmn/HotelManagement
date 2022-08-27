using HotelManagement.Data;
using HotelManagement.Training.Context;
using HotelManagement.Training.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Training.UnitOfWorks
{
    public class TrainingUnitOfWork : UnitOfWork, ITrainingUnitOfWork
    {
        public IUserRepository Users { get; private set; }

        public IRoomRepository Rooms { get; private set; }

        public IBookingRepository Bookings { get; private set; }

        public TrainingUnitOfWork(ITrainingContext context,
            IUserRepository users,
            IRoomRepository rooms,
            IBookingRepository bookings
            ) : base((DbContext)context)
        {
            Users = users;
            Rooms = rooms;
            Bookings = bookings;
        }
    }
}
