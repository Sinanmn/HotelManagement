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
    public class UserListModel
    {
        private IUserService _userService;
        private IHttpContextAccessor _httpContextAccessor;
        public UserListModel()
        {
            _userService = Startup.AutofacContainer.Resolve<IUserService>();
            _httpContextAccessor = Startup.AutofacContainer.Resolve<IHttpContextAccessor>();
        }

        public UserListModel(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        internal object GetUsers(DataTablesAjaxRequestModel tableModel)
        {
            var session = _httpContextAccessor.HttpContext.Session;

            var data = _userService.GetUsers(
                tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "UName", "UPone", "UEmail","UPass", "UAddress" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {

                            record.Id.ToString(),
                            record.UName,
                            record.UPhone,
                            record.UEmail,
                            record.UPass,
                            record.UAddress,
                            record.Id.ToString(),

                        }
                        ).ToArray()

            };
        }

        internal void Delete(int id)
        {
            _userService.DeleteUser(id);
        }

    }
}
