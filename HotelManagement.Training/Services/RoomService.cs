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
    public class RoomService : IRoomService
    {
        private readonly ITrainingUnitOfWork _trainingUnitOfWork;
        private readonly IDateTimeUtility _dateTimeUtility;
        private readonly IMapper _mapper;
        public RoomService(ITrainingUnitOfWork trainingUnitOfWork,
            IDateTimeUtility dateTimeUtility,
            IMapper mapper)
        {
            _trainingUnitOfWork = trainingUnitOfWork;
            _dateTimeUtility = dateTimeUtility;
            _mapper = mapper;

        }
        public void AddRoom(Room room)
        {
            if (room == null)
                throw new invalidPerameterException("Room details was not provided");

            _trainingUnitOfWork.Rooms.Add(
            _mapper.Map<Entities.Room>(room)
        );

            _trainingUnitOfWork.Save();
        }

        public void DeleteRoom(int id)
        {
            _trainingUnitOfWork.Rooms.Remove(id);
            _trainingUnitOfWork.Save();
        }

        public IList<Room> GetAllRooms()
        {
            var roomEntities = _trainingUnitOfWork.Rooms.GetAll();
            var rooms = new List<Room>();

            foreach (var entity in roomEntities)
            {
                var room = _mapper.Map<Room>(entity);
                rooms.Add(room);
            }

            return rooms;
        }

        public Room GetRoom(int id)
        {
            var room = _trainingUnitOfWork.Rooms.GetById(id);

            if (room == null) return null;

            return _mapper.Map<Room>(room);
        }

        public (IList<Room> records, int total, int totalDisplay) GetRooms(int pageIndex, int pageSize, string searchText, string sortText)
        {
            var roomData = _trainingUnitOfWork.Rooms.GetDynamic(string.IsNullOrWhiteSpace(searchText) ? null :
               x => x.RName.Contains(searchText), sortText, string.Empty,
                pageIndex, pageSize);

            var resultData = (from room in roomData.data
                              select _mapper.Map<Room>(room)).ToList();

            return (resultData, roomData.total, roomData.totalDisplay);
        }

        public void UpdateRoom(Room room)
        {
            if (room == null)
                throw new InvalidOperationException("Room is missing");


            var roomEntity = _trainingUnitOfWork.Rooms.GetById(room.Id);

            if (roomEntity != null)
            {
                _mapper.Map(room, roomEntity);

                _trainingUnitOfWork.Save();

            }
            else
                throw new InvalidOperationException("Couldn't find room");
        }
    }
}
