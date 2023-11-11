using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.DTO.User;

namespace Application.Models.Validators.User
{
    public class UpdateReqUserVal : AbstractValidator<UpdateUserDTO>
    {
        public UpdateReqUserVal()
        {
            RuleFor(e => e.Id)
                .NotEmpty()
                .GreaterThan(0)
                .WithErrorCode("400")
                .WithMessage("Id must be not empty and less than 0");

            RuleFor(e => e.Name)
                .NotEmpty()
                .WithErrorCode("400")
                .WithMessage("Name must be not empty");

            RuleFor(e => e.Age)
                .NotEmpty().InclusiveBetween(1, 100)
                .WithErrorCode("400")
                .WithMessage("Age must be not empty and must have length between 1 and 100.");

        }
    }
}
