using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Account;
using PartyMaker.Common.Translator;
using PartyMaker.Common.Validation;
using PartyMaker.Configuration.Interfaces;
using PartyMaker.Domain.Entities;
using PartyMaker.Dto.WebApp;
using Microsoft.IdentityModel.Tokens;
using PartyMaker.Common.Approver;
using PartyMaker.Story.CommonStories;

namespace PartyMaker.Story.WebApplication
{
    public class WebAppAuthenticateStory : RequestStory<WebAppAuthenticateStoryContext, WebAppResponseWithEntityDto<WebAppAuthenticateResponceDto>>, IStory<WebAppAuthenticateStoryContext, WebAppResponseWithEntityDto<WebAppAuthenticateResponceDto>>
    {
        private readonly IRepository _repository;
        private readonly ICrudService _crudService;
        private readonly IIdentityService _identityService;
        private readonly IAuthenticationSettings _authenticationSettings;
        private readonly ITranslatorFactory _translatorFactory;

        public WebAppAuthenticateStory(
            IRepository repository,
            ICrudService crudService,
            IIdentityService identityService,
            IAuthenticationSettings authenticationSettings,
            IWebAppValidatorFactory webAppValidatorFactory,
            ITranslatorFactory translatorFactory,
            IWebApproverFactory approverFactory,
            IAppLogger appLogger)
            : base(approverFactory, webAppValidatorFactory, appLogger)
        {
            _repository = repository;
            _crudService = crudService;
            _identityService = identityService;
            _authenticationSettings = authenticationSettings;
            _translatorFactory = translatorFactory;
        }

        protected override async Task<WebAppResponseWithEntityDto<WebAppAuthenticateResponceDto>> Run(WebAppAuthenticateStoryContext context)
        {
            var responce = new WebAppResponseWithEntityDto<WebAppAuthenticateResponceDto>()
            {
                IsSuccess = true,
                Result = new WebAppAuthenticateResponceDto
                {
                    WrongCredentials = false,
                },
            };

            
            var user = await _repository.GetAsync<User>(t => t.Email == context.Email);
            user.RefreshToken = Guid.NewGuid();
            await _crudService.UpdateAsync(user);
            
            var translator = _translatorFactory.GetTranslator<User, WebAppAuthenticateResponceDto>();

            responce.Result = translator.Translate(user);


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authenticationSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = _identityService.GetClaimsIdentity(user.Id, user.UserRole, context.Password),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            responce.Result.Token = tokenHandler.WriteToken(token);
            responce.Result.RefreshToken = user.RefreshToken.ToString();
            responce.Result.ExpiresTime = (long)(tokenDescriptor.Expires.Value - DateTime.UtcNow).TotalSeconds;
            
            responce.IsSuccess = true;
            return responce;
        }
    }
}
