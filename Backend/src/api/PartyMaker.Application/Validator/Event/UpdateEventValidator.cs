using FluentValidation;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Story.WebApplication.Event;
using System;

namespace PartyMaker.Application.Validator.Event
{
    public class UpdateEventValidator : AbstractValidator<WebAppUpdateEventStoryContext>
    {
        public UpdateEventValidator()
        {
            RuleFor(t => t.Name)
               .NotEmpty()
               .WithMessage(WebAppErrors.NameEventIsNullOrEmtpy)
               .Length(1, 100)
               .WithMessage(WebAppErrors.NameEvetnMustbeFrom1To50Symbols);


            RuleFor(t => t.Date)
                .GreaterThan(DateTime.Today)
                .WithMessage(WebAppErrors.DateEventCannotBeEarlierThenToday);

            RuleFor(t => t.Latitude)
                .GreaterThanOrEqualTo(-90)
                .WithMessage(WebAppErrors.LatitudeNotCorrect)
                .LessThanOrEqualTo(90)
                .WithMessage(WebAppErrors.LatitudeNotCorrect);

            RuleFor(t => t.Longitude)
                .GreaterThanOrEqualTo(0)
                .WithMessage(WebAppErrors.LongitudeNotCorrect)
                .LessThanOrEqualTo(180)
                .WithMessage(WebAppErrors.LongitudeNotCorrect);

            RuleFor(t => t.Address)
                .MaximumLength(255)
                .WithMessage(WebAppErrors.LengthOfAddressMustBeLess);

            RuleFor(t => t.TotalBudget)
                .LessThanOrEqualTo(99999999999)
                .WithMessage(WebAppErrors.TotalBudgetMustBeLess);
        }
    }
}
