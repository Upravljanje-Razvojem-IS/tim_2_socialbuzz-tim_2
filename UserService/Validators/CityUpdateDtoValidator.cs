using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Dtos.Cities;

namespace UserService.Validators
{
    public class CityUpdateDtoValidator: AbstractValidator<CityUpdateDto>
    {
        public CityUpdateDtoValidator()
        {
            RuleFor(x => x.CityName).NotNull().NotEmpty().MaximumLength(50).MinimumLength(3);

        }
    }
}
