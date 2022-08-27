using HotelManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Training.Entities
{
    public class Room : IEntity<int>
    {
        public int Id { get; set; }
        public string RName { get; set; }
        public string RLocation { get; set; }
        public string RCost { get; set; }
        public string Status { get; set; }
    }
}
