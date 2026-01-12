
using FluentValidation;
using ZHSystem.Application.Features.Users.Queries;

namespace ZHSystem.Application.Validators.Users
{
    public class GetUsersQueryValidatorL :AbstractValidator<GetUsersQuery>
    {
        private static readonly string[] AllowedSortFields =
        {
            "id",
            "username",
            "email",
            "createdat"
        };
        private bool BeAValidSortField(string? sortBy)
        {
            return AllowedSortFields.Contains(sortBy!.ToLower());
        }

        public GetUsersQueryValidatorL()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageNumber must be >= 1");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 50)
                .WithMessage("PageSize must be between 1 and 50");

            RuleFor(x => x.SortBy)
                .Must(BeAValidSortField)
                .When(x => !string.IsNullOrWhiteSpace(x.SortBy))
                .WithMessage($"SortBy must be one of: {string.Join(", ", AllowedSortFields)}");

            RuleFor(x => x.SearchUserName)
                .MinimumLength(2)
                .When(x => !string.IsNullOrWhiteSpace(x.SearchUserName));

            RuleFor(x => x.SearchEmail)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.SearchEmail));

            RuleFor(x => x.SearchRole)
                .MinimumLength(2)
                .When(x => !string.IsNullOrWhiteSpace(x.SearchRole));
        }
       
    }
}
