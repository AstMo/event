using bgTeam.DataAccess;
using FluentValidation;
using PartyMaker.Common.ErrorProvider;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Domain.Entities;
using PartyMaker.Story.WebApplication.Task;
using System;
using System.Threading.Tasks;

namespace PartyMaker.Application.Validator.Event
{
    public class CreateTaskValidator : AbstractValidator<WebAppCreateTaskStoryContext>
    {

        public CreateTaskValidator()
        {

            RuleFor(t => t.Name)
               .NotEmpty()
               .WithMessage(WebAppErrors.NameEventIsNullOrEmtpy)
               .Length(1, 50)
               .WithMessage(WebAppErrors.NameEvetnMustbeFrom1To50Symbols);

            RuleFor(t => t.Description)
                .MaximumLength(1000)
                .WithMessage(WebAppErrors.DescriptionTaskMaximum);
        }
    }
}
