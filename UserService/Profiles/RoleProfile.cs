using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Dtos;
using UserService.Dtos.Roles;
using UserService.Entities;

namespace UserService.Profiles
{
    public class RoleProfile: Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>();
            CreateMap<RoleCreationDto, Role>();
            CreateMap<RoleUpdateDto, Role>();
            CreateMap<Role, RoleCreatedConfirmation>();
            CreateMap<RoleCreatedConfirmation, RoleCreatedConfirmationDto>();
            CreateMap<Role, Role>();
        }
    }
}
