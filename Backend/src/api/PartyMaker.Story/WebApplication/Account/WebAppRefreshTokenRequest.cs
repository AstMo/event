using bgTeam;
using bgTeam.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using PartyMaker.Common.Account;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Translator;
using PartyMaker.Common.Validation;
using PartyMaker.Configuration.Interfaces;
using PartyMaker.Domain.Entities;
using PartyMaker.Dto.WebApp;
using PartyMaker.Story.CommonStories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PartyMaker.Story.WebApplication.Account
{
    class WebAppRefreshTokenRequest : RequestStory<WebAppRefreshTokenRequestContext, WebAppResponseWithEntityDto<WebAppAuthenticateResponceDto>>, IStory<WebAppRefreshTokenRequestContext, WebAppResponseWithEntityDto<WebAppAuthenticateResponceDto>>
    {
        private readonly IRepository _repository;
        private readonly ICrudService _crudService;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthenticationSettings _authenticationSettings;
        private readonly ITranslatorFactory _translatorFactory;

        public WebAppRefreshTokenRequest(
            IRepository repository,
            ICrudService crudService,
            IIdentityService identityService,
            IHttpContextAccessor httpContextAccessor,
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
            _httpContextAccessor = httpContextAccessor;
            _authenticationSettings = authenticationSettings;
            _translatorFactory = translatorFactory;
        }

        protected override async Task<WebAppResponseWithEntityDto<WebAppAuthenticateResponceDto>> Run(WebAppRefreshTokenRequestContext context)
        {
            var responce = new WebAppResponseWithEntityDto<WebAppAuthenticateResponceDto>()
            {
                IsSuccess = true,
                Result = new WebAppAuthenticateResponceDto
                {
                    WrongCredentials = false,
                },
            };

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            var user = await _repository.GetAsync<User>(t => t.Id == Guid.Parse(userId));

            user.RefreshToken = Guid.NewGuid();
            await _crudService.UpdateAsync(user);

            var translator = _translatorFactory.GetTranslator<User, WebAppAuthenticateResponceDto>();

            responce.Result = translator.Translate(user);


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authenticationSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = _identityService.GetClaimsIdentity(user.Id, user.UserRole, user.Name),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            responce.Result.Token = tokenHandler.WriteToken(token);
            responce.Result.RefreshToken = user.RefreshToken.ToString();
            responce.Result.ExpiresTime = (long)(DateTime.UtcNow - tokenDescriptor.Expires.Value).TotalSeconds;

            responce.IsSuccess = true;
            return responce;
        }
    }
}
