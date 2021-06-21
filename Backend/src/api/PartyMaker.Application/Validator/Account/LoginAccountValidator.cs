using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Story.WebApplication;
using FluentValidation;

namespace PartyMaker.Application.Validator.Account
{
    public class LoginAccountValidator : AbstractValidator<WebAppAuthenticateStoryContext>
    {
        public LoginAccountValidator()
        {
            RuleFor(t => t.Email)
                .NotEmpty()
                .WithMessage(WebAppErrors.UsernameIsNullOrEmtpy)
                .Length(3, 100)
                .WithMessage(WebAppErrors.UsernameNotCorrectLength);

            RuleFor(t => t.Password)
                .NotEmpty()
                .WithMessage(WebAppErrors.PasswordIsNullOrEmtpy)
                .Length(3, 20)
                .WithMessage(WebAppErrors.PasswordNotCorrectLength);
        }
    }
}
