using bgTeam.DataAccess;
using FluentValidation;
using PartyMaker.Common.Account;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Story.WebApplication.Account;
using System.Linq;
using System.Threading.Tasks;

namespace PartyMaker.Application.Approvers.Account
{
    public class ApproveEmailUserApprover : RequestApprover<ApproveUserEmailStoryContext>
    {
        private readonly IAccountService _accountService;
        private readonly IRepository _repository;
        private readonly IConnectionFactory _connectionFactory;

        public ApproveEmailUserApprover(
            IAccountService accountService,
            IRepository repository,
            IConnectionFactory connectionFactory)
        {
            _accountService = accountService;
            _repository = repository;
            _connectionFactory = connectionFactory;

            RuleFor(t => t)
                .MustAsync((context, ct) => TokenIsCorrect(context.Token))
                .WithMessage(WebAppErrors.UserCannotBeApproved);
        }

        private async Task<bool> TokenIsCorrect(string token)
        {
            using var connection = await _connectionFactory.CreateAsync();

            return (await _repository.GetAllAsync<Domain.Entities.User>(t => t.LinkHash == token && !t.IsActive)).Count() == 1;
        }
    }
}
