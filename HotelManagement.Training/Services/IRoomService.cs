using HotelManagement.Training.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Training.Services
{
    public interface IRoomService
    {
        IList<Room> GetAllRooms();

        void AddRoom(Room room);

        (IList<Room> records, int total, int totalDisplay) GetRooms(int pageIndex,
            int pageSize, string searchText, string sortText);

        Room GetRoom(int id);
        void UpdateRoom(Room room);
        void DeleteRoom(int id);
    }
}
