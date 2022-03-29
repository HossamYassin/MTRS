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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public List<UserListItem> GetList()
        {
            return _userRepository.GetList();
        }

        public List<UserDto> GetAll()
        {
            return _mapper.Map<List<UserDto>>(_userRepository.GetAll());
        }

        public UserDto GetById(long id)
        {
            return _mapper.Map<UserDto>(_userRepository.GetById(id));
        }

        public void Add(UserDto userDto)
        {
            var entity = _mapper.Map<User>(userDto);
            _userRepository.Add(entity);
        }

        public IList<UserDto> Get(Expression<Func<User, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize)
        {
            return _mapper.Map<List<UserDto>>(_userRepository.Find(expression, sortColumn, isSortAscending, page, pageSize).ToList());
        }

        public void Remove(UserDto userDto)
        {
            var entity = _mapper.Map<User>(userDto);
            _userRepository.Remove(entity);
        }

        public void Update(UserDto userDto)
        {
            var user = _userRepository.GetById(userDto.Id);

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.IsActive = userDto.IsActive;
            user.IsAdmin = userDto.IsAdmin;
            user.IsManager = userDto.IsManager;
            user.IsProjectManager = userDto.IsProjectManager;
            user.IsSuperManager = userDto.IsSuperManager;
            user.DepartmentId = userDto.DepartmentId;
            user.PositionId = userDto.PositionId;
            user.ManagerId = userDto.ManagerId;
            user.AllowTimeSheet = userDto.AllowTimeSheet;
            user.GradeId = userDto.GradeId;

            _userRepository.Update();
        }

        public int Count()
        {
            return _userRepository.Count();
        }

        public List<UserDto> GetMyEmployees(long managerId, int level)
        {
            return _mapper.Map<List<UserDto>>(_userRepository.GetMyEmployees(managerId, level));
        }

        public UserDashboardDto GetDashboard(long userId)
        {
            return _userRepository.GetDashboard(userId);
        }

        public TeamDashboardDto GetTeamDashboard(long managerId)
        {
            return _userRepository.GetTeamDashboard(managerId);
        }
    }
}
