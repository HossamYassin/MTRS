using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MTRS.Services.Interfaces
{
    public interface IUserService
    {
        List<UserListItem> GetList();
        List<UserDto> GetAll();
        UserDto GetById(long id);
        IList<UserDto> Get(Expression<Func<User, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize);
        void Update(UserDto userDto);
        void Remove(UserDto userDto);
        void Add(UserDto userDto);
        int Count();
        List<UserDto> GetMyEmployees(long managerId, int level);
        UserDashboardDto GetDashboard(long userId);
        TeamDashboardDto GetTeamDashboard(long managerId);
    }
}
