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
    public class RoomListModel
    {
        int ab;
        private IRoomService _roomService;
        private IHttpContextAccessor _httpContextAccessor;
        public RoomListModel()
        {
            _roomService = Startup.AutofacContainer.Resolve<IRoomService>();
            _httpContextAccessor = Startup.AutofacContainer.Resolve<IHttpContextAccessor>();
        }

        public RoomListModel(IRoomService roomService, IHttpContextAccessor httpContextAccessor)
        {
            _roomService = roomService;
            _httpContextAccessor = httpContextAccessor;
        }

        internal object GetRooms(DataTablesAjaxRequestModel tableModel)
        {
            var session = _httpContextAccessor.HttpContext.Session;

            var data = _roomService.GetRooms(
                tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "RName", "RLocation", "RCost", "Status" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {

                            record.Id.ToString(),
                            record.RName,
                            record.RLocation,
                            record.RCost,
                            record.Status,
                            record.Id.ToString(),

                        }
                        ).ToArray()

            };
        }

        internal void Delete(int id)
        {
            _roomService.DeleteRoom(id);
        }
    }
}
