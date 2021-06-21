using FluentValidation;
using PartyMaker.Story.WebApplication.Account;

namespace PartyMaker.Application.Validator.Account
{
    public class ApproveEmailValidator : AbstractValidator<ApproveUserEmailStoryContext>
    {
        public ApproveEmailValidator()
        {
        }
    }
}
