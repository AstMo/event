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
    public class RefreshTokenApprover : RequestApprover<WebAppRefreshTokenRequestContext>
    {
        private readonly IRepository _repository;
        private readonly IConnectionFactory _connectionFactory;

        public RefreshTokenApprover(
            IRepository repository,
            IConnectionFactory connectionFactory)
        {
            _repository = repository;
            _connectionFactory = connectionFactory;


            RuleFor(t => t.Token)
                .MustAsync((context, ct) => CheckUserExist(context))
                .WithMessage(WebAppErrors.EntityNotFound);
        }

        private async Task<bool> CheckUserExist(Guid id)
        {
            using var connection = await _connectionFactory.CreateAsync();

            return (await _repository.GetAllAsync<User>(t => t.RefreshToken == id)).FirstOrDefault() != null;
        }
    }
}
