using FluentValidation;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Story.WebApplication.Account;

namespace PartyMaker.Application.Validator.Account
{
    public class UpdateUserValidator : AbstractValidator<WebAppUpdateUserStoryContext>
    {

        public UpdateUserValidator()
        {
            RuleFor(t => t.Name)
               .NotEmpty()
               .WithMessage(WebAppErrors.UsernameIsNullOrEmtpy)
               .Length(3, 20)
               .WithMessage(WebAppErrors.UsernameNotCorrectLength);

            RuleFor(t => t.Email)
                .NotEmpty()
                .WithMessage(WebAppErrors.EmailCannotBeEmpty)
                .Length(3, 100)
                .WithMessage(WebAppErrors.EmailNotCorrectLength)
                .EmailAddress()
                .WithMessage(WebAppErrors.EmailNotCorrect);

            RuleFor(t => t.Phone)
                .MaximumLength(15)
                .WithMessage(WebAppErrors.PhoneCannotBeMoreLength);
        }
    }
}
