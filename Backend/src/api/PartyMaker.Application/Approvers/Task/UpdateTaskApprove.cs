using bgTeam.DataAccess;
using FluentValidation;
using PartyMaker.Common.Approver;
using PartyMaker.Common.ErrorProvider;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Domain.Entities;
using PartyMaker.Story.WebApplication.Task;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PartyMaker.Application.Approvers.Event
{
    public class UpdateTaskApprove : RequestApprover<WebAppUpdateTaskStoryContext>
    {
        private readonly IErrorMessageProvider _errorMessageProvider;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IRepository _repository;

        public UpdateTaskApprove(
            IConnectionFactory connectionFactory,
            IRepository repository)
        {
            _connectionFactory = connectionFactory;
            _repository = repository;

            RuleFor(t => t.Id)
                .MustAsync((context, ct) => CheckEntityIsExist(context))
                .WithMessage(WebAppErrors.EntityNotFound);


            RuleFor(t => t.EventId)
                .MustAsync((t, ct) => EventExist(t))
                .WithMessage(WebAppErrors.EventNotExist);


            RuleFor(t => t.AssignedId)
                .MustAsync((t, ct) => UserExist(t))
                .WithMessage(WebAppErrors.UserNotFound);
        }

        private async Task<bool> CheckEntityIsExist(Guid id)
        {
            using var connection = await _connectionFactory.CreateAsync();

            return (await _repository.GetAllAsync<Domain.Entities.TaskEvent>(t => id == t.Id && t.IsDeleted == false)).Count() == 1;
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
