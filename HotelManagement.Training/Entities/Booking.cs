using HotelManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Training.Entities
{
    public class Booking : IEntity<int>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string BDate { get; set; }
        public string BRoom { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public string Amount { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
    }
}
