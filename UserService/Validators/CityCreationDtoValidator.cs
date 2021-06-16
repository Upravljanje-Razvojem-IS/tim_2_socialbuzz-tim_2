using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Dtos.Cities;

namespace UserService.Validators
{
    public class CityCreationDtoValidator: AbstractValidator<CityCreationDto>
    {
        public CityCreationDtoValidator()
        {
            RuleFor(x => x.CityName).NotNull().NotEmpty().MaximumLength(50).MinimumLength(3);

        }
    }
}
