using FluentValidation;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Story.WebApplication.Account;

namespace PartyMaker.Application.Validator.Account
{
    public class RestoreUserValidator : AbstractValidator<WebAppRestorePasswordStoryContext>
    {
        public RestoreUserValidator()
        {
            RuleFor(t => t.Email)
                .NotEmpty()
                .WithMessage(WebAppErrors.UsernameIsNullOrEmtpy)
                .Length(3, 100)
                .WithMessage(WebAppErrors.UsernameNotCorrectLength);
        }
    }
}