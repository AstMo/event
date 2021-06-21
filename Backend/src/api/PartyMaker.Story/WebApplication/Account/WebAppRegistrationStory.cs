using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Account;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Translator;
using PartyMaker.Common.Validation;
using PartyMaker.Domain.Entities;
using PartyMaker.Dto.Queue;
using PartyMaker.Dto.WebApp;
using PartyMaker.Story.CommonStories;
using System;
using System.Threading.Tasks;

namespace PartyMaker.Story.WebApplication
{
    public class WebAppRegistrationStory : RequestStory<WebAppRegistrationStoryContext, WebAppResponseDto>, IStory<WebAppRegistrationStoryContext, WebAppResponseDto>
    {

        private readonly IConnectionFactory _connectionFactory;
        private readonly ICrudService _crudService;
        private readonly ITranslatorFactory _translatorFactory;
        private readonly IAccountService _tokenService;
        private readonly IAppLogger _appLogger;

        public WebAppRegistrationStory(
            IConnectionFactory connectionFactory,
            ICrudService crudService,
            ITranslatorFactory translatorFactory,
            IAccountService tokenService,
            IWebAppValidatorFactory webAppValidatorFactory,
            IWebApproverFactory webApprover,
            IAppLogger appLogger)
            : base(webApprover, webAppValidatorFactory, appLogger)
        {
            _connectionFactory = connectionFactory;
            _crudService = crudService;
            _translatorFactory = translatorFactory;
            _tokenService = tokenService;
            _appLogger = appLogger;
        }

        protected override async Task<WebAppResponseDto> Run(WebAppRegistrationStoryContext context)
        {
            try
            {
                var translator = _translatorFactory.GetTranslator<WebAppRegistrationStoryContext, User>();
                var user = translator.Translate(context);
                user.MarkAsNew();

                _tokenService.CreatePasswordHash(context.Password, out byte[] passwordHash, out byte[] passwordSalt);


                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.IsActive = false;
                _tokenService.GenerateLinkHash(context.Email, context.Password, DateTime.Now, out byte[] hash);
                user.LinkHash = Convert.ToBase64String(hash).Replace('/', '_').Replace('+', '-');

                user.MarkAsNew();
                using (var dbConnection = await _connectionFactory.CreateAsync())
                {
                    await _crudService.InsertAsync(user, dbConnection);
                }

                return new WebAppResponseDto
                {
                    IsSuccess = true,
                    Message = string.Empty,
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
