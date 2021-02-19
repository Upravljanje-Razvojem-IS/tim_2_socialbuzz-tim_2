using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.Data;
using UserService.Dtos;
using UserService.Entities;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IPersonalUserRepository personalUserRepository;
        private readonly ICorporationUserRepository corporationUserRepository;
        private readonly IMapper mapper;

        public UserController(IPersonalUserRepository personalUserRepository, ICorporationUserRepository corporationUserRepository, IMapper mapper)
        {
            this.personalUserRepository = personalUserRepository;
            this.corporationUserRepository = corporationUserRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<UserDto>> GetUsers(string userType, string city)
        {
            if (string.IsNullOrEmpty(userType))
            {
                List<Corporation> corporations = corporationUserRepository.GetUsers(city);
                List<PersonalUser> personalUsers = personalUserRepository.GetUsers(city); 
                List<UserDto> users = new List<UserDto>();
                users.AddRange(mapper.Map<List<PersonalUserDto>>(personalUsers));
                users.AddRange(mapper.Map<List<CorporationDto>>(corporations));
                return Ok(users);
            }
            else
            {
                if (string.Equals(userType, "personalUser"))
                {

                }
                else if (string.Equals(userType, "corporationUser"))
                {

                }
                else
                {
                    //not found
                }
            }

            
            return Ok(null);
        }
    }
}
