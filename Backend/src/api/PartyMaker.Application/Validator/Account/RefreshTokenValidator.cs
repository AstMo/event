using FluentValidation;
using PartyMaker.Story.WebApplication.Account;

namespace PartyMaker.Application.Validator.Account
{
    public class RefreshTokenValidator: AbstractValidator<WebAppRefreshTokenRequestContext>
    {
        public RefreshTokenValidator()
        {

        }
    }
}
