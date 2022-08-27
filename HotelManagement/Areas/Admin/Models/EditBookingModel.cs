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
    public class EditBookingModel
    {
        public int? Id { get; set; }
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

        public EditBookingModel()
        {
            _bookingService = Startup.AutofacContainer.Resolve<IBookingService>();
        }

        public void LoadModelData(int id)
        {
            var booking = _bookingService.GetBooking(id);
            Id = booking?.Id;
            UserId = (int)booking?.UserId;
            BDate = booking?.BDate;
            BRoom = booking?.BRoom;
            CheckIn = booking?.CheckIn;
            CheckOut = booking?.CheckOut;
            Amount = booking?.Amount;
            TransactionId = booking?.TransactionId;
            Status = booking?.Status;
        }

        internal void Update()
        {
            var booking = new Booking
            {
                Id = Id.HasValue ? Id.Value : 0,
                UserId = UserId,
                BDate = BDate,
                BRoom = BRoom,
                CheckIn= CheckIn,
                CheckOut = CheckOut,
                Amount= Amount,
                TransactionId = TransactionId,
                Status= Status

            };
            _bookingService.UpdateBooking(booking);
        }
    }
}
