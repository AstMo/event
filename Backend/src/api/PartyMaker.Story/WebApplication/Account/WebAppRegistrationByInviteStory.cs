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
    class WebAppRegistrationByInviteStory : RequestStory<WebAppRegistrationByInviteStoryContext, WebAppResponseDto>, IStory<WebAppRegistrationByInviteStoryContext, WebAppResponseDto>
    {

        private readonly IConnectionFactory _connectionFactory;
        private readonly ICrudService _crudService;
        private readonly IRepository _repository;
        private readonly ITranslatorFactory _translatorFactory;
        private readonly IAccountService _tokenService;
        private readonly IAppLogger _appLogger;

        public WebAppRegistrationByInviteStory(
            IConnectionFactory connectionFactory,
            ICrudService crudService,
            IRepository repository,
            ITranslatorFactory translatorFactory,
            IAccountService tokenService,
            IWebAppValidatorFactory webAppValidatorFactory,
            IWebApproverFactory webApprover,
            IAppLogger appLogger)
            : base(webApprover, webAppValidatorFactory, appLogger)
        {
            _connectionFactory = connectionFactory;
            _crudService = crudService;
            _repository = repository;
            _translatorFactory = translatorFactory;
            _tokenService = tokenService;
            _appLogger = appLogger;
        }

        protected override async Task<WebAppResponseDto> Run(WebAppRegistrationByInviteStoryContext context)
        {
            try
            {
                using var dbConnection = await _connectionFactory.CreateAsync();
                var user = await _repository.GetAsync<User>(t => t.Email == context.Email, dbConnection);
                var translator = _translatorFactory.GetTranslator<WebAppRegistrationByInviteStoryContext, Domain.Entities.User>();
                translator.Update(context, user);
                user.MarkAsUpdated();

                _tokenService.CreatePasswordHash(context.Password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.IsActive = false;
                _tokenService.GenerateLinkHash(context.Email, context.Password, DateTime.Now, out byte[] hash);
                user.LinkHash = Convert.ToBase64String(hash).Replace('/', '_').Replace('+', '-');

                await _crudService.UpdateAsync(user, dbConnection);

                return new WebAppResponseDto
                {
                    IsSuccess = true,
                    Message = WebAppSuccessMessages.RegistrationCorrect,
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
