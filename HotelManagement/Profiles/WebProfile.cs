using AutoMapper;
using HotelManagement.Areas.Admin.Models;
using HotelManagement.Training.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagement.Profiles
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<AddRoomModel, Room>().ReverseMap();
            CreateMap<AddUserModel, User>().ReverseMap();
            CreateMap<AddBookingModel, Booking>().ReverseMap();
        }
    }
}
