using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Account;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Email;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Common.Validation;
using PartyMaker.Dto.WebApp;
using PartyMaker.Story.CommonStories;
using System;
using System.Threading.Tasks;

namespace PartyMaker.Story.WebApplication.Account
{
    class ResendApprovingUserEmailStory : RequestStory<ResendApprovingUserEmailStoryContext, WebAppResponseDto>, IStory<ResendApprovingUserEmailStoryContext, WebAppResponseDto>
    {

        private readonly IConnectionFactory _connectionFactory;
        private readonly IRepository _repository;
        private readonly ICrudService _crudService;
        private readonly IWebAppEmailSenderService _webAppEmailSenderService;
        private readonly IAccountService _tokenService;
        private readonly IAppLogger _appLogger;

        public ResendApprovingUserEmailStory(
            IConnectionFactory connectionFactory,
            IRepository repository,
            ICrudService crudService,
            IWebAppEmailSenderService webAppEmailSenderService,
            IAccountService tokenService,
            IWebAppValidatorFactory webAppValidatorFactory,
            IWebApproverFactory webApprover,
            IAppLogger appLogger)
            : base(webApprover, webAppValidatorFactory, appLogger)
        {
            _connectionFactory = connectionFactory;
            _repository = repository;
            _crudService = crudService;
            _webAppEmailSenderService = webAppEmailSenderService;
            _tokenService = tokenService;
            _appLogger = appLogger;
        }

        protected override async Task<WebAppResponseDto> Run(ResendApprovingUserEmailStoryContext context)
        {
            try
            {
                var isOk = await _webAppEmailSenderService.SendRegistrationInfo(context.Email);

                return new WebAppResponseDto
                {
                    IsSuccess = isOk,
                    Message = WebAppSuccessMessages.EmailResend,
                };
            }
            catch (Exception ex)
            {
                _appLogger.Error(ex);
                return new WebAppResponseDto
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
