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
    public class EditUserModel
    {
        public int? Id { get; set; }
        [Required, MaxLength(200, ErrorMessage = "Name should be less than 200 characters")]
        public string UName { get; set; }
        public string UPhone { get; set; }
        public string UEmail { get; set; }
        public string UPass { get; set; }
        public string UAddress { get; set; }



        private readonly IUserService _userService;
        public EditUserModel()
        {
            _userService = Startup.AutofacContainer.Resolve<IUserService>();
        }

        public void LoadModelData(int id)
        {
            var user = _userService.GetUser(id);
            Id = user?.Id;
            UName = user?.UName;
            UPhone = user?.UPhone;
            UEmail = user?.UEmail;
            UPass = user?.UPass;
            UAddress = user?.UAddress;
        }

        internal void Update()
        {
            var user = new User
            {
                Id = Id.HasValue ? Id.Value : 0,
                UName = UName,
                UPhone = UPhone,
                UEmail= UEmail,
                UPass= UPass,
                UAddress = UAddress

            };
            _userService.UpdateUser(user);
        }
    }
}
