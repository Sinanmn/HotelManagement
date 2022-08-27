using HotelManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Training.Entities
{
    public class User : IEntity<int>
    {
        public int Id { get; set; }
        public string UName { get; set; }
        public string UPhone { get; set; }
        public string UEmail { get; set; }
        public string UPass { get; set; }
        public string UAddress { get; set; }
    }
}
