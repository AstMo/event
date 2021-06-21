using bgTeam.DataAccess;
using FluentValidation;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Story.WebApplication.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartyMaker.Application.Approvers.Event
{
    public class UpdateEventApprove : RequestApprover<WebAppUpdateEventStoryContext>
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IRepository _repository;

        public UpdateEventApprove(
            IConnectionFactory connectionFactory,
            IRepository repository)
        {
            _connectionFactory = connectionFactory;
            _repository = repository;

            RuleFor(t => t.Id)
                .MustAsync((context, ct) => CheckEntityIsExist(context))
                .WithMessage(WebAppErrors.EntityNotFound);
        }

        private async Task<bool> CheckEntityIsExist(Guid id)
        {
            using var connection = await _connectionFactory.CreateAsync();

            return (await _repository.GetAllAsync<Domain.Entities.Event>(t => id == t.Id && t.IsDeleted == false)).Count() == 1;
        }
    }
}
