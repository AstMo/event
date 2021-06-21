using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Account;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Common.Translator;
using PartyMaker.Common.Validation;
using PartyMaker.Domain.Entities;
using PartyMaker.Dto.WebApp;
using PartyMaker.Story.CommonStories;
using System;
using System.Threading.Tasks;

namespace PartyMaker.Story.WebApplication.Account
{
    public class WebAppInvitePersonByEmailStory : RequestStory<WebAppInvitePersonByEmailStoryContext, WebAppResponseDto>, IStory<WebAppInvitePersonByEmailStoryContext, WebAppResponseDto>
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ICrudService _crudService;
        private readonly IAppLogger _appLogger;

        public WebAppInvitePersonByEmailStory(
            IConnectionFactory connectionFactory,
            ICrudService crudService,
            IWebAppValidatorFactory webAppValidatorFactory,
            IWebApproverFactory webApprover,
            IAppLogger appLogger)
            : base(webApprover, webAppValidatorFactory, appLogger)
        {
            _connectionFactory = connectionFactory;
            _crudService = crudService;
            _appLogger = appLogger;
        }

        protected override async Task<WebAppResponseDto> Run(WebAppInvitePersonByEmailStoryContext context)
        {
            try
            {
                var user = new User
                {
                    Email = context.Email,
                    IsActive = false
                };
                user.MarkAsNew();

                using var dbConnection = await _connectionFactory.CreateAsync();
                await _crudService.InsertAsync(user, dbConnection);

                return new WebAppResponseDto
                {
                    IsSuccess = true,
                    Message = WebAppSuccessMessages.PersonInvited,
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
