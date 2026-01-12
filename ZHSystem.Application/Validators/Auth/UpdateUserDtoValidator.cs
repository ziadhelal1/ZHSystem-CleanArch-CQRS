using FluentValidation;
using ZHSystem.Application.DTOs.Auth;


namespace ZHSystem.Application.Validators.Auth
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            When(x => x.UserName != null, () => RuleFor(x => x.UserName!).MinimumLength(3));
            When(x => x.Email != null, () => RuleFor(x => x.Email!).EmailAddress());
            When(x => x.NewPassword != null, () => RuleFor(x => x.NewPassword!).MinimumLength(6));
        }
    }
}
