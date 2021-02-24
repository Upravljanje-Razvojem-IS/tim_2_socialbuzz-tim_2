using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Dtos.Users;

namespace UserService.Validators
{
    public class PersonalUserUpdateDtoValidator: AbstractValidator<PersonalUserUpdateDto>
    {
        public PersonalUserUpdateDtoValidator()
        {
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.FirstName).NotNull().NotEmpty().MaximumLength(50).MinimumLength(3);
            RuleFor(x => x.LastName).NotNull().NotEmpty().MaximumLength(50).MinimumLength(3);
            RuleFor(x => x.Telephone).NotNull().NotEmpty().MaximumLength(30).MinimumLength(7);
            RuleFor(x => x.Username).NotNull().NotEmpty().MaximumLength(50).MinimumLength(3);
            RuleFor(x => x.IsActive).NotNull();
            RuleFor(x => x.CityId).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(5);
        }
    }
}
