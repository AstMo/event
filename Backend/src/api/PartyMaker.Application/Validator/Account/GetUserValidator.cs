using FluentValidation;
using PartyMaker.Story.WebApplication;

namespace PartyMaker.Application.Validator.Account
{
    public class GetUserValidator : AbstractValidator<WebAppGetAuthUserStoryContext>
    {
        public GetUserValidator()
        {
        }
    }
}
