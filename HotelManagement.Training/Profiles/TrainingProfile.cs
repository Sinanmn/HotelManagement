using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EO = HotelManagement.Training.Entities;
using BO = HotelManagement.Training.BusinessObjects;
using AutoMapper;

namespace HotelManagement.Training.Profiles
{
    public class TrainingProfile : Profile
    {
        public TrainingProfile()
        {
            CreateMap<EO.User, BO.User>().ReverseMap();
            CreateMap<EO.Room, BO.Room>().ReverseMap();
            CreateMap<EO.Booking, BO.Booking>().ReverseMap();
        }
        
    }
}
