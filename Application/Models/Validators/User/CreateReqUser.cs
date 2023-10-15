using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.DTO.User;

namespace Application.Models.Validators.User
{
    public class CreateReqUser : AbstractValidator<CreateReqUserDTO>
    {
        public CreateReqUser()
        {
            RuleFor(e => e.Id)
                .NotEmpty()
                .GreaterThan(0)
                .WithErrorCode("400")
                .WithMessage("Id must be not empty and greater than 0");

            RuleFor(e => e.Name)
                .NotEmpty()
                .WithErrorCode("400")
                .WithMessage("Name must be not empty"); 

            RuleFor(e => e.Age)
                .NotEmpty().InclusiveBetween(1, 100)
                .WithErrorCode("400")
                .WithMessage("Age must be not empty and have length between 1 and 100.");
                
        }
    }
}
