using bgTeam;
using PartyMaker.Dto.WebApp;
using PartyMaker.Story.WebApplication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using PartyMaker.Story.WebApplication.Account;
using PartyMaker.Common.Email;
using bgTeam.DataAccess;
using PartyMaker.Dto.WebApp.Users;
using PartyMaker.DataAccess.Users;
using System;
using PartyMaker.Common.Impl.ErrorProvider;

namespace PartyMaker.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IStoryBuilder _storyBuilder;
        private readonly IQueryBuilder _queryBuilder;
        private readonly IWebAppEmailSenderService _webAppEmailSenderService;
        private readonly IAppLogger _appLogger;

        public AccountController(IStoryBuilder storyBuilder,
            IQueryBuilder queryBuilder,
            IWebAppEmailSenderService webAppEmailSenderService,
            IAppLogger appLogger)
        {
            _storyBuilder = storyBuilder;
            _queryBuilder = queryBuilder;
            _webAppEmailSenderService = webAppEmailSenderService;
            _appLogger = appLogger;
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(WebAppAuthenticateResponceDto), 200)]
        [ProducesResponseType(typeof(WebAppAuthenticateResponceDto), 400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Login([FromBody] WebAuthentocationRequestDto requestDto)
        {
            _appLogger.Info($"Get request login");
            var result = await _storyBuilder.Build(new WebAppAuthenticateStoryContext { Email = requestDto.Email, Password = requestDto.Password })
                .ReturnAsync<WebAppResponseWithEntityDto<WebAppAuthenticateResponceDto>>();

            return result.IsSuccess || result.IsInvalid ? GetActionResult(result) : StatusCode((int)HttpStatusCode.Forbidden, result);
        }

        [HttpGet]
        [Authorize]
        [Route("refresh/{refreshToken:guid}")]
        [ProducesResponseType(typeof(WebAppAuthenticateResponceDto), 200)]
        [ProducesResponseType(typeof(WebAppAuthenticateResponceDto), 400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Refresh(Guid refreshToken)
        {
            _appLogger.Info("Get refresh request");
            var result = await _storyBuilder.Build(new WebAppRefreshTokenRequestContext { Token = refreshToken })
                .ReturnAsync<WebAppResponseWithEntityDto<WebAppAuthenticateResponceDto>>();
            return result.IsSuccess || result.IsInvalid ? GetActionResult(result) : StatusCode((int)HttpStatusCode.Forbidden, result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("restore")]
        [ProducesResponseType(typeof(WebAppResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppResponseDto), 400)]
        public async Task<IActionResult> RestorePassword([FromBody] WebAppRestorePasswordDto restorePasswordDto)
        {
            _appLogger.Info("Get restore password request");
            var result = await _storyBuilder.Build(new WebAppRestorePasswordStoryContext { Email = restorePasswordDto.Email })
                .ReturnAsync<WebAppResponseDto>();

            return GetActionResult(result);
        }

        [HttpGet]
        [Authorize]
        [Route("search/{searchQuery}")]
        [ProducesResponseType(typeof(WebAppUserTableItemDto), 200)]
        public async Task<IActionResult> GetUsersWithSearch(string searchQuery)
        {
            _appLogger.Info($"Get request get users with search query {searchQuery}");
            var result = await _queryBuilder.Build(new WebAppGetUsersBySearchQueryContext { SearchQuery = searchQuery })
                .ReturnAsync<WebAppResponseWithTableDto<WebAppTableDto<WebAppUserTableItemDto>, WebAppUserTableItemDto>>();

            return GetActionResult(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("registration")]
        [ProducesResponseType(typeof(WebAppResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppResponseDto), 400)]
        public async Task<IActionResult> Registration([FromBody] WebAppRegistrationRequestDto registrationDto)
        {
            _appLogger.Info($"Get request registration");
            var result = await _storyBuilder.Build(new WebAppRegistrationStoryContext
            {
                Email = registrationDto.Email,
                Password = registrationDto.Password,
                PasswordRepeat = registrationDto.PasswordRepeat,
                ImageId = registrationDto.ImageId,
                Name = registrationDto.Name,
                Phone = registrationDto.Phone,
                Birthday = registrationDto.Birthday,
            }).ReturnAsync<WebAppResponseDto>();

            var actionResult = GetActionResult(result);
            if (result.IsSuccess)
            {
                var isOk = await _webAppEmailSenderService.SendRegistrationInfo(registrationDto.Email);
                if (!isOk)
                {
                    return StatusCode((int)HttpStatusCode.Conflict, result);
                }
            }
            return actionResult;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("registrationbyinvite")]
        [ProducesResponseType(typeof(WebAppResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppResponseDto), 400)]
        public async Task<IActionResult> RegistrationByInvite([FromBody] WebAppRegistrationRequestDto registrationDto)
        {
            _appLogger.Info($"Get request registration by invite");
            var result = await _storyBuilder.Build(new WebAppRegistrationByInviteStoryContext
            {
                Email = registrationDto.Email,
                Password = registrationDto.Password,
                PasswordRepeat = registrationDto.PasswordRepeat,
                ImageId = registrationDto.ImageId,
                Name = registrationDto.Name,
                Phone = registrationDto.Phone,
                Birthday = registrationDto.Birthday,
            }).ReturnAsync<WebAppResponseDto>();

            var actionResult = GetActionResult(result);
            if (result.IsSuccess)
            {
                var isOk = await _webAppEmailSenderService.SendRegistrationInfo(registrationDto.Email);
                if (!isOk)
                {
                    return StatusCode((int)HttpStatusCode.Conflict, result);
                }
            }
            return actionResult;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("approve/{linkToken}")]
        [ProducesResponseType(typeof(WebAppResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppResponseDto), 400)]
        public async Task<IActionResult> Approve(string linkToken)
        {
            _appLogger.Info($"Get request approve registration");
            var result = await _storyBuilder.Build(new ApproveUserEmailStoryContext
            {
                Token = linkToken
            }).ReturnAsync<WebAppResponseDto>();

            return GetActionResult(result);
        }

        [HttpPost]
        [AllowAnonymous] 
        [Route("resend")]
        [ProducesResponseType(typeof(WebAppResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppResponseDto), 400)]
        public async Task<IActionResult> Resend([FromBody] WebAppResendEmailDto resendDto)
        {
            _appLogger.Info($"Get request resend request");
            var result = await _storyBuilder.Build(new ResendApprovingUserEmailStoryContext
            {
                Email = resendDto.Email
            }).ReturnAsync<WebAppResponseDto>();

            return GetActionResult(result);
        }

        [HttpPut]
        [Authorize]
        [Route("update")]
        [ProducesResponseType(typeof(WebAppResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppResponseDto), 400)]
        public async Task<IActionResult> UpdateUserInfo([FromBody] WebAppUpdateUserRequestDto updateRequestDto)
        {
            _appLogger.Info($"Get request update user info");
            var result = await _storyBuilder.Build(new WebAppUpdateUserStoryContext
            {
                Id = updateRequestDto.Id,
                Email = updateRequestDto.Email,
                ImageId = updateRequestDto.ImageId,
                Name = updateRequestDto.Name,
                Phone = updateRequestDto.Phone,
                Birthday = updateRequestDto.Birthday
            }).ReturnAsync<WebAppResponseDto>();

            return GetActionResult(result);
        }

        [HttpPost]
        [Authorize]
        [Route("logout")]
        [ProducesResponseType(200)]
        public IActionResult Logout()
        {
            _appLogger.Info($"Get request logout request");
            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpGet]
        [Authorize]
        [Route("self")]
        [ProducesResponseType(typeof(WebAppAuthUserResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppAuthUserResponseDto), 400)]
        public async Task<IActionResult> Self()
        {
            _appLogger.Info($"Get request auth user");
            var result = await _storyBuilder.Build(new WebAppGetAuthUserStoryContext())
               .ReturnAsync<WebAppResponseWithEntityDto<WebAppAuthUserResponseDto>>();

            return GetActionResult(result);
        }
    }
}
