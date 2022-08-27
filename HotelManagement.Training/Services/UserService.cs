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
    public class UserService : IUserService
    {
        private readonly ITrainingUnitOfWork _trainingUnitOfWork;
        private readonly IDateTimeUtility _dateTimeUtility;
        private readonly IMapper _mapper;
        public UserService(ITrainingUnitOfWork trainingUnitOfWork,
            IDateTimeUtility dateTimeUtility,
            IMapper mapper)
        {
            _trainingUnitOfWork = trainingUnitOfWork;
            _dateTimeUtility = dateTimeUtility;
            _mapper = mapper;

        }
        public void AddUser(User user)
        {
            if (user == null)
                throw new invalidPerameterException("User details was not provided");

            _trainingUnitOfWork.Users.Add(
           _mapper.Map<Entities.User>(user)
       );

            _trainingUnitOfWork.Save();
        }

        public void DeleteUser(int id)
        {
            _trainingUnitOfWork.Users.Remove(id);
            _trainingUnitOfWork.Save();
        }

        public IList<User> GetAllUsers()
        {
            var userEntities = _trainingUnitOfWork.Users.GetAll();
            var users = new List<User>();

            foreach (var entity in userEntities)
            {
                var user = _mapper.Map<User>(entity);
                users.Add(user);
            }

            return users;
        }

        public User GetUser(int id)
        {
            var user = _trainingUnitOfWork.Users.GetById(id);

            if (user == null) return null;

            return _mapper.Map<User>(user);
        }

        public (IList<User> records, int total, int totalDisplay) GetUsers(int pageIndex, int pageSize, string searchText, string sortText)
        {
            var userData = _trainingUnitOfWork.Users.GetDynamic(string.IsNullOrWhiteSpace(searchText) ? null :
              x => x.UName.Contains(searchText), sortText, string.Empty,
               pageIndex, pageSize);

            var resultData = (from user in userData.data
                              select _mapper.Map<User>(user)).ToList();

            return (resultData, userData.total, userData.totalDisplay);
        }

        public void UpdateUser(User user)
        {
            if (user == null)
                throw new InvalidOperationException("User is missing");


            var userEntity = _trainingUnitOfWork.Users.GetById(user.Id);

            if (userEntity != null)
            {
                _mapper.Map(user, userEntity);

                _trainingUnitOfWork.Save();

            }
            else
                throw new InvalidOperationException("Couldn't find user");
        }
    }
}
