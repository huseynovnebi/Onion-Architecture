using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.DTO.Auth;

namespace Application.Models.Validators.User
{
    public class LoginCreds : AbstractValidator<LoginAuthDto>
    {
        public LoginCreds()
        {
            RuleFor(e => e.UserName)
                .NotEmpty()
                .WithErrorCode("400")
                .WithMessage("UserName must be not empty");

            RuleFor(e => e.Password)
                .NotEmpty()
                .WithErrorCode("400")
                .WithMessage("Password must be not empty.");

        }
    }
}
