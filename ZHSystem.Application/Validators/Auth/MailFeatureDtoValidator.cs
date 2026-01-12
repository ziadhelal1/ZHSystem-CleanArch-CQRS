using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ZHSystem.Application.DTOs.Auth;

namespace ZHSystem.Application.Validators.Auth
{
    public class MailFeatureDtoValidator : AbstractValidator<MailFeatureDto>
    {
        public MailFeatureDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
