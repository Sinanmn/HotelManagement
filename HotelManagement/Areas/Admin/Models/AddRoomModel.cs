using Autofac;
using AutoMapper;
using HotelManagement.Training.BusinessObjects;
using HotelManagement.Training.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagement.Areas.Admin.Models
{
    public class AddRoomModel
    {
        [Required, MaxLength(200, ErrorMessage = "Name should be less than 200 characters")]
        public string RName { get; set; }
        public string RLocation { get; set; }
        public string RCost { get; set; }
        public string Status { get; set; }

        private readonly IRoomService _roomService;

        private readonly IMapper _mapper;

        public AddRoomModel()
        {
            _roomService = Startup.AutofacContainer.Resolve<IRoomService>();
            _mapper = Startup.AutofacContainer.Resolve<IMapper>();
        }
        public AddRoomModel(IRoomService roomService)
        {
            _roomService = roomService;
        }

        internal void AddRoom()
        {
            var room = _mapper.Map<Room>(this);

            _roomService.AddRoom(room);

        }
    }
}
