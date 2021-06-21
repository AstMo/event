using FluentValidation;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Story.WebApplication.Account;

namespace PartyMaker.Application.Validator.Account
{
    public class RegistrationByInviteValidator : AbstractValidator<WebAppRegistrationByInviteStoryContext>
    {
        public RegistrationByInviteValidator()
        {
            RuleFor(t => t.Name)
               .NotEmpty()
               .WithMessage(WebAppErrors.UsernameIsNullOrEmtpy)
               .Length(3, 20)
               .WithMessage(WebAppErrors.UsernameNotCorrectLength);

            RuleFor(t => t)
                .Must(t => t.Password == t.PasswordRepeat)
                .WithMessage(WebAppErrors.PasswordNotEqual);


            RuleFor(t => t.Password)
                .NotEmpty()
                .WithMessage(WebAppErrors.PasswordIsNullOrEmtpy)
                .Length(3, 20)
                .WithMessage(WebAppErrors.PasswordNotCorrectLength);

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
