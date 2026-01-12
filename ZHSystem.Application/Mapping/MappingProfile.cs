using AutoMapper;
using ZHSystem.Application.DTOs;
using ZHSystem.Domain.Entities;
using ZHSystem.Application.DTOs.Auth;
using ZHSystem.Application.DTOs.UserMangment;
using ZHSystem.Application.Common.Extensions;

namespace ZHSystem.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 1. Student mapping - Ignores ID and Soft Delete properties
            CreateMap<CreateStudentDto, Student>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .IgnoreSoftDeleteProperties();

            // 2. User mapping - Flattens roles list into string array
            CreateMap<User, UserWithRolesDto>()
                .ForMember(dest => dest.Roles, opt =>
                    opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)));

            // 3. Register mapping - Ignores sensitive fields and skips null values
            CreateMap<RegisterDto, User>()
                .ApplyIgnoreList()
                .ForAllMembers(opts => {
                    opts.Condition((src, dest, srcMember) => srcMember != null);
                });

            // 4. Create User mapping - Standard security ignore list
            CreateMap<CreateUserDto, User>()
                .ApplyIgnoreList();

            // 5. Role mapping - Ignores ID and navigation properties
            CreateMap<CreateRoleDto, Role>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .IgnoreSoftDeleteProperties();
            

        }
    }
}