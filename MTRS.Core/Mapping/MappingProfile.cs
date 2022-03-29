using AutoMapper;
using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTRS.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Activity, ActivityDto>().ReverseMap();
            CreateMap<ActivityUser, ActivityUserDto>().ReverseMap();

            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Position, PositionDto>().ReverseMap();
            CreateMap<Department, DepartmentDto>().ReverseMap();

            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<NLog, NLogDto>().ReverseMap();

            CreateMap<TimeSheet, TimeSheetDto>().ReverseMap();
            CreateMap<TimeSheetDetails, TimeSheetDetailsDto>().ReverseMap();
            CreateMap<TimeSheetApproval, TimeSheetApprovalDto>().ReverseMap();

            CreateMap<Grade, GradeDto>().ReverseMap();
            CreateMap<BaseActivity, BaseActivityDto>().ReverseMap();
        }
    }
}
