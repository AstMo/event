using bgTeam.DataAccess;
using FluentValidation;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Domain.Entities;
using PartyMaker.Story.WebApplication.Account;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PartyMaker.Application.Approvers.Account
{
    public class UpdateUserApprover : RequestApprover<WebAppUpdateUserStoryContext>
    {
        private readonly IRepository _repository;
        private readonly IConnectionFactory _connectionFactory;

        public UpdateUserApprover(
            IRepository repository,
            IConnectionFactory connectionFactory)
        {
            _repository = repository;
            _connectionFactory = connectionFactory;


            RuleFor(t => t.Id)
                .MustAsync((context, ct) => CheckUserExist(context))
                .WithMessage(WebAppErrors.EntityNotFound);
        }

        private async Task<bool> CheckUserExist(Guid id)
        {
            using var connection = await _connectionFactory.CreateAsync();

            return (await _repository.GetAllAsync<User>(t => t.Id == id)).FirstOrDefault() != null;
        }
    }
}
