using FluentValidation;
using PartyMaker.Story.WebApplication.Event;

namespace PartyMaker.Application.Validator.Event
{
    public class DeleteEventValidator : AbstractValidator<WebAppDeleteEventStoryContext>
    {

        public DeleteEventValidator()
        {
            
        }
    }
}
