using bgTeam.DataAccess;
using FluentValidation;
using PartyMaker.Common.Approver;
using PartyMaker.Common.ErrorProvider;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Domain.Entities;
using PartyMaker.Story.WebApplication.Task;
using System;
using System.Threading.Tasks;

namespace PartyMaker.Application.Approvers.Event
{
    public class CreateTaskApprover : RequestApprover<WebAppCreateTaskStoryContext>
    {
        private readonly IRepository _repository;

        public CreateTaskApprover(
            IRepository repository,
            IConnectionFactory connectionFactory)
        {
            _repository = repository;
            RuleFor(t => t.EventId)
                .MustAsync((context, ct) => EventExist(context))
                .WithMessage(WebAppErrors.EntityNotFound);

            RuleFor(t => t.AssignedId)
                .MustAsync((context, ct) => UserExist(context))
                .WithMessage(WebAppErrors.EntityNotFound);
        }

        private async Task<bool> UserExist(Guid id)
        {
            return (await _repository.GetAsync<User>(t => t.Id == id)) != null;
        }

        private async Task<bool> EventExist(Guid id)
        {
            return (await _repository.GetAsync<Domain.Entities.Event>(t => t.Id == id)) != null;
        }
    }
}
