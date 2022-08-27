using HotelManagement.Training.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Training.Services
{
    public interface IBookingService
    {
        IList<Booking> GetAllBookings();

        void AddBooking(Booking booking);

        (IList<Booking> records, int total, int totalDisplay) GetBookings(int pageIndex,
            int pageSize, string searchText, string sortText);

        Booking GetBooking(int id);
        void UpdateBooking(Booking booking);
        void DeleteBooking(int id);
    }
}
