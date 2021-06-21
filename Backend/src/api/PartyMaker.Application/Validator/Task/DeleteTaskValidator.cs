using FluentValidation;
using PartyMaker.Story.WebApplication.Task;

namespace PartyMaker.Application.Validator.Task
{
    public class DeleteTaskValidator : AbstractValidator<WebAppDeleteTaskStoryContext>
    {

        public DeleteTaskValidator()
        {
            
        }
    }
}
