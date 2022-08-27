using AutoMapper;
using HotelManagement.Common.Utilities;
using HotelManagement.Training.BusinessObjects;
using HotelManagement.Training.Exceptions;
using HotelManagement.Training.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Training.Services
{
    public class BookingService : IBookingService
    {
        private readonly ITrainingUnitOfWork _trainingUnitOfWork;
        private readonly IDateTimeUtility _dateTimeUtility;
        private readonly IMapper _mapper;
        public BookingService(ITrainingUnitOfWork trainingUnitOfWork,
            IDateTimeUtility dateTimeUtility,
            IMapper mapper)
        {
            _trainingUnitOfWork = trainingUnitOfWork;
            _dateTimeUtility = dateTimeUtility;
            _mapper = mapper;

        }

        public void AddBooking(Booking booking)
        {
            if (booking == null)
                throw new invalidPerameterException("Booking details was not provided");

            _trainingUnitOfWork.Bookings.Add(
            _mapper.Map<Entities.Booking>(booking)
            );

            _trainingUnitOfWork.Save();
        }

        public void DeleteBooking(int id)
        {
            _trainingUnitOfWork.Bookings.Remove(id);
            _trainingUnitOfWork.Save();
        }

        public IList<Booking> GetAllBookings()
        {
            var bookingEntities = _trainingUnitOfWork.Bookings.GetAll();
            var bookings = new List<Booking>();

            foreach (var entity in bookingEntities)
            {
                var booking = _mapper.Map<Booking>(entity);
                bookings.Add(booking);
            }

            return bookings;
        }

        public Booking GetBooking(int id)
        {
            var booking = _trainingUnitOfWork.Bookings.GetById(id);

            if (booking == null) return null;

            return _mapper.Map<Booking>(booking);
        }

        public (IList<Booking> records, int total, int totalDisplay) GetBookings(int pageIndex, int pageSize, string searchText, string sortText)
        {
            var bookingData = _trainingUnitOfWork.Bookings.GetDynamic(string.IsNullOrWhiteSpace(searchText) ? null :
               x => x.BDate.Contains(searchText), sortText, string.Empty,
                pageIndex, pageSize);

            var resultData = (from booking in bookingData.data
                              select _mapper.Map<Booking>(booking)).ToList();

            return (resultData, bookingData.total, bookingData.totalDisplay);
        }

        public void UpdateBooking(Booking booking)
        {
            if (booking == null)
                throw new InvalidOperationException("Booking is missing");


            var bookingEntity = _trainingUnitOfWork.Bookings.GetById(booking.Id);

            if (bookingEntity != null)
            {
                _mapper.Map(booking, bookingEntity);

                _trainingUnitOfWork.Save();

            }
            else
                throw new InvalidOperationException("Couldn't find booking");
        }
    }
}
