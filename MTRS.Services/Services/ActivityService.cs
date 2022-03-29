using AutoMapper;
using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using MTRS.DAL.Interfaces;
using MTRS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MTRS.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IProjectUserRepository _projectUserRepository;
        private readonly IMapper _mapper;

        public ActivityService(IActivityRepository activityRepository,
            IProjectUserRepository projectUserRepository, IMapper mapper)
        {
            _mapper = mapper;
            _activityRepository = activityRepository;
            _projectUserRepository = projectUserRepository;
        }

        public List<ActivityDto> GetList(long projectId)
        {
            return _mapper.Map<List<ActivityDto>>(_activityRepository.GetList(projectId));
        }

        public List<ActivityDto> GetAll()
        {
            return _mapper.Map<List<ActivityDto>>(_activityRepository.GetAll());
        }

        public ActivityDto GetById(long id)
        {
            return _mapper.Map<ActivityDto>(_activityRepository.GetById(id));
        }

        public void Add(ActivityDto activityDto)
        {
            var entity = _mapper.Map<Activity>(activityDto);
            _activityRepository.Add(entity);
        }

        public IList<ActivityDto> Get(Expression<Func<Activity, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize)
        {
            return _mapper.Map<List<ActivityDto>>(_activityRepository.Find(expression, sortColumn, isSortAscending, page, pageSize).ToList());
        }

        public void Remove(ActivityDto activityDto)
        {
            var entity = _mapper.Map<Activity>(activityDto);
            _activityRepository.Remove(entity);
        }

        public void Update(ActivityDto activityDto)
        {
            var activity = _activityRepository.GetById(activityDto.Id);
            _mapper.Map(activityDto, activity);

            _activityRepository.Update();
        }

        public int Count()
        {
            return _activityRepository.Count();
        }

        public List<ActivityDto> GetGeneral()
        {
            return _mapper.Map<List<ActivityDto>>(_activityRepository.GetGeneral());
        }

        public List<ActivityDto> GetUserProjectActivity(long projectId, long userId)
        {
            return _mapper.Map<List<ActivityDto>>(_activityRepository.GetUserProjectActivity(projectId, userId));
        }

        public List<ActivityDto> GetByProjectId(long projectId)
        {
            return _mapper.Map<List<ActivityDto>>(_activityRepository.GetByProjectId(projectId));
        }

        public List<UserDto> GetAssignedEmployeesByActivityId(long activityId)
        {
            return _mapper.Map<List<UserDto>>(_activityRepository.GetAssignedEmployeesByActivityId(activityId));
        }

        public int AssignEmployeeToActivity(ActivityUserDto activityUser)
        {
            var userActvity = _mapper.Map<ActivityUser>(activityUser);

            return _projectUserRepository.AssignEmployeeToActivity(userActvity);
        }

        public bool UnassignUser(ActivityUserDto activityUser)
        {
            var userActvity = _mapper.Map<ActivityUser>(activityUser);

            return _projectUserRepository.UnassignUser(userActvity);
        }

        public List<ActivityDto> GetUserActivities(long projectId, long userId)
        {
            return _mapper.Map<List<ActivityDto>>(_activityRepository.GetUserActivities(projectId, userId));
        }
    }
}
