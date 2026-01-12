using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ZHSystem.Application.DTOs.Auth;

namespace ZHSystem.Application.Validators.Auth
{
    public class ResetPasswordDtoValidator:AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);

        }
    }
}
