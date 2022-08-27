using HotelManagement.Training.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Training.Services
{
    public interface IUserService
    {
        IList<User> GetAllUsers();

        void AddUser(User user);

        (IList<User> records, int total, int totalDisplay) GetUsers(int pageIndex,
            int pageSize, string searchText, string sortText);

        User GetUser(int id);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}
