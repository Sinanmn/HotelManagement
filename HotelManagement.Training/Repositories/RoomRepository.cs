using HotelManagement.Data;
using HotelManagement.Training.Context;
using HotelManagement.Training.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Training.Repositories
{
    public class RoomRepository : Repository<Room,int>,
        IRoomRepository
    {
        public RoomRepository(ITrainingContext context)
            :base((DbContext)context)
        {

        }
    }
}
