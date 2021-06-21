using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Common.Validation;
using PartyMaker.Dto.WebApp;
using PartyMaker.Story.CommonStories;
using System.Threading.Tasks;

namespace PartyMaker.Story.WebApplication.Account
{
    public class ApproveUserEmailStory : RequestStory<ApproveUserEmailStoryContext, WebAppResponseDto>, IStory<ApproveUserEmailStoryContext, WebAppResponseDto>
    {
        private readonly IRepository _repository;
        private readonly ICrudService _crudService;
        private readonly IConnectionFactory _connectionFactory;

        public ApproveUserEmailStory(
            IRepository repository,
            ICrudService crudService,
            IConnectionFactory connectionFactory,
            IWebAppValidatorFactory webAppValidatorFactory,
            IWebApproverFactory approverFactory,
            IAppLogger appLogger)
            : base(approverFactory, webAppValidatorFactory, appLogger)
        {
            _repository = repository;
            _crudService = crudService;
            _connectionFactory = connectionFactory;
        }

        protected override async Task<WebAppResponseDto> Run(ApproveUserEmailStoryContext context)
        {
            using var dbConnection = await _connectionFactory.CreateAsync();
            var user = await _repository.GetAsync<Domain.Entities.User>(t => t.LinkHash == context.Token, dbConnection);
            user.IsActive = true;
            await _crudService.UpdateAsync(user, dbConnection);
            
            return new WebAppResponseDto()
            {
                IsSuccess = true,
                Message = WebAppSuccessMessages.UserApproved
            };
        }
    }
}
