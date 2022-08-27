using Autofac;
using HotelManagement.Training.BusinessObjects;
using HotelManagement.Training.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagement.Areas.Admin.Models
{
    public class EditRoomModel
    {
        public int? Id { get; set; }
        [Required, MaxLength(200, ErrorMessage = "Name should be less than 200 characters")]
        public string RName { get; set; }
        public string RLocation { get; set; }
        public string RCost { get; set; }
        public string Status { get; set; }

        private readonly IRoomService _roomService;

        public EditRoomModel()
        {
            _roomService = Startup.AutofacContainer.Resolve<IRoomService>();
        }

        public void LoadModelData(int id)
        {
            var room = _roomService.GetRoom(id);
            Id = room?.Id;
            RName = room?.RName;
            RLocation = room?.RLocation;
            RCost = room?.RCost;
            Status = room?.Status;
        }

        internal void Update()
        {
            var room = new Room
            {
                Id = Id.HasValue ? Id.Value : 0,
                RName = RName,
                RLocation = RLocation,
                RCost = RCost,
                Status = Status

            };
            _roomService.UpdateRoom(room);
        }
    }
}
