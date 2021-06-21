using bgTeam.DataAccess;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using PartyMaker.Common.Approver;
using PartyMaker.Common.ErrorProvider;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Domain.Entities;
using PartyMaker.Story.WebApplication.Task;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PartyMaker.Application.Approvers.Event
{
    public class DeleteTaskApprover : RequestApprover<WebAppDeleteTaskStoryContext>
    {
        private readonly IRepository _repository;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteTaskApprover(
            IHttpContextAccessor httpContextAccessor,
            IRepository repository,
            IConnectionFactory connectionFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _connectionFactory = connectionFactory;

            RuleFor(t => t.Id)
                .MustAsync((context, ct) => CheckEntityIsExist(context))
                .WithMessage(WebAppErrors.EntityNotFound)
                .MustAsync((context, ct) => RightsCorrect(context))
                .WithMessage(WebAppErrors.UserDoesntHaveRights);
        }

        private async Task<bool> CheckEntityIsExist(Guid id)
        {
            using var connection = await _connectionFactory.CreateAsync();

            return (await _repository.GetAllAsync<Domain.Entities.TaskEvent>(t => id == t.Id && t.IsDeleted == false)).Count() == 1;
        }

        private async Task<bool> RightsCorrect(Guid id)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value);
            var task = await _repository.GetAsync<Domain.Entities.TaskEvent>(t => t.Id == id);
            return (await _repository.GetAllAsync<Domain.Entities.UserEvent>(t => t.EventId == task.EventId))
                .Any(t => t.Id == userId && t.Role.HasFlag(EUserEventRole.Admin));
        }
    }
}
