using bgTeam.DataAccess;
using FluentValidation;
using PartyMaker.Common.Account;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Domain.Entities;
using PartyMaker.Story.WebApplication.Account;
using System.Linq;
using System.Threading.Tasks;

namespace PartyMaker.Application.Approvers.Account
{
    public class RestoreUserApprover : RequestApprover<WebAppRestorePasswordStoryContext>
    {
        private readonly IAccountService _accountService;
        private readonly IRepository _repository;
        private readonly IConnectionFactory _connectionFactory;

        public RestoreUserApprover(
            IAccountService accountService,
            IRepository repository,
            IConnectionFactory connectionFactory)
        {
            _accountService = accountService;
            _repository = repository;
            _connectionFactory = connectionFactory;


            RuleFor(t => t.Email)
                .MustAsync((context, ct) => CheckUserNotExist(context))
                .WithMessage(WebAppErrors.UserNotFound);
        }

        private async Task<bool> CheckUserNotExist(string email)
        {
            using var connection = await _connectionFactory.CreateAsync();

            return (await _repository.GetAllAsync<User>(t => t.Email == email && t.IsActive)).Count() == 1;
        }
    }
}

