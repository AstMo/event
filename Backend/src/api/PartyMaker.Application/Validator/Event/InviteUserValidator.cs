using FluentValidation;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Story.WebApplication.Account;

namespace PartyMaker.Application.Validator.Event
{
    public class InviteUserValidator : AbstractValidator<WebAppInvitePersonByEmailStoryContext>
    {

        public InviteUserValidator()
        {
            RuleFor(t => t.Email)
                .NotEmpty()
                .WithMessage(WebAppErrors.EmailCannotBeEmpty)
                .Length(3, 100)
                .WithMessage(WebAppErrors.EmailNotCorrectLength)
                .EmailAddress()
                .WithMessage(WebAppErrors.EmailNotCorrect);

        }
    }
}
