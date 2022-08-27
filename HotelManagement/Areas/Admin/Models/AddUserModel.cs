
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
    public class AddUserModel
    {
        [Required, MaxLength(200, ErrorMessage = "Name should be less than 200 characters")]
        public string UName { get; set; }
        public string UPhone { get; set; }
        public string UEmail { get; set; }
        public string UPass { get; set; }
        public string UAddress { get; set; }



        private readonly IUserService _userService;

        private readonly IMapper _mapper;

        public AddUserModel()
        {
            _userService = Startup.AutofacContainer.Resolve<IUserService>();
            _mapper = Startup.AutofacContainer.Resolve<IMapper>();

        }

        public AddUserModel(IUserService userService)
        {
            _userService = userService;
        }

        internal void AddUser()
        {
            var user = _mapper.Map<User>(this);

            _userService.AddUser(user);

        }
    }
}
