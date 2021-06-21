using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Account;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Email;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Common.Validation;
using PartyMaker.Domain.Entities;
using PartyMaker.Dto.WebApp;
using PartyMaker.Story.CommonStories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PartyMaker.Story.WebApplication.Account
{
    class WebAppRestorePasswordStory : RequestStory<WebAppRestorePasswordStoryContext, WebAppResponseDto>, IStory<WebAppRestorePasswordStoryContext, WebAppResponseDto>
    {

        private readonly IConnectionFactory _connectionFactory;
        private readonly IRepository _repository;
        private readonly ICrudService _crudService;
        private readonly IWebAppEmailSenderService _webAppEmailSenderService;
        private readonly IAccountService _tokenService;
        private readonly IAppLogger _appLogger;

        public WebAppRestorePasswordStory(
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

        protected override async Task<WebAppResponseDto> Run(WebAppRestorePasswordStoryContext context)
        {
            try
            {
                bool isOk;
                using (var dbConnection = await _connectionFactory.CreateAsync())
                {
                        var user = _repository.Get<User>(t => t.Email == context.Email, dbConnection);
                    var createdPassword = _tokenService.GeneratePassword();
                    _tokenService.CreatePasswordHash(createdPassword, out byte[] passwordHash, out byte[] passwordSalt);


                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;

                    user.MarkAsUpdated();
                    await _crudService.UpdateAsync(user, dbConnection);

                    isOk = await _webAppEmailSenderService.SendRestoreInfo(context.Email, createdPassword);
                }

                return new WebAppResponseDto
                {
                    IsSuccess = isOk,
                    Message = WebAppSuccessMessages.RestoreCorrect
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
