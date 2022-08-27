using Autofac;
using HotelManagement.Common.Utilities;
using HotelManagement.Training.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagement.Areas.Admin.Models
{
    public class BookingListModel
    {
        private IBookingService _bookingService;
        private IHttpContextAccessor _httpContextAccessor;
        public BookingListModel()
        {
            _bookingService = Startup.AutofacContainer.Resolve<IBookingService>();
            _httpContextAccessor = Startup.AutofacContainer.Resolve<IHttpContextAccessor>();
        }

        public BookingListModel(IBookingService doctorService, IHttpContextAccessor httpContextAccessor)
        {
            _bookingService = doctorService;
            _httpContextAccessor = httpContextAccessor;
        }

        internal object GetBookings(DataTablesAjaxRequestModel tableModel)
        {
            var session = _httpContextAccessor.HttpContext.Session;

            var data = _bookingService.GetBookings(
                tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "UserId", "BDate", "BRoom", "CheckIn", "CheckOut", "Amount", "TransactionId", "Status" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {

                            record.UserId.ToString(),
                            record.BDate,
                            record.BRoom,
                            record.CheckIn,
                            record.CheckOut,
                            record.Amount,
                            record.TransactionId,
                            record.Status,
                            record.Id.ToString(),

                        }
                        ).ToArray()

            };
        }

        internal void Delete(int id)
        {
            _bookingService.DeleteBooking(id);
        }

    }
}
