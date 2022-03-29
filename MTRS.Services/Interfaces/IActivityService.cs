using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MTRS.Services.Interfaces
{
    public interface IActivityService
    {
        List<ActivityDto> GetAll();
        ActivityDto GetById(long id);
        IList<ActivityDto> Get(Expression<Func<Activity, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize);
        void Update(ActivityDto ActivityDto);
        void Remove(ActivityDto ActivityDto);
        void Add(ActivityDto ActivityDto);
        int Count();
        List<ActivityDto> GetList(long projectId);
        List<ActivityDto> GetGeneral();
        List<ActivityDto> GetUserProjectActivity(long projectId, long userId);
        List<ActivityDto> GetByProjectId(long projectId);
        List<UserDto> GetAssignedEmployeesByActivityId(long activityId);
        int AssignEmployeeToActivity(ActivityUserDto activityUser);
        bool UnassignUser(ActivityUserDto activityUser);
        List<ActivityDto> GetUserActivities(long projectId, long userId);
    }
}
