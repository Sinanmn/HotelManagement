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
    public class AddBookingModel
    {
        [Required]
        public int UserId { get; set; }
        public string BDate { get; set; }
        public string BRoom { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public string Amount { get; set; }
        [Required]
        public string TransactionId { get; set; }
        public string Status { get; set; }

        private readonly IBookingService _bookingService;

        private readonly IMapper _mapper;

        public AddBookingModel()
        {
            _bookingService = Startup.AutofacContainer.Resolve<IBookingService>();
            _mapper = Startup.AutofacContainer.Resolve<IMapper>();
        }

        public AddBookingModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        internal void AddBooking()
        {
            var booking = _mapper.Map<Booking>(this);

            _bookingService.AddBooking(booking);

        }
    }
}
