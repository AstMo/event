using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Domain.Entities;
using PartyMaker.Dto.WebApp;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using PartyMaker.Story.CommonStories;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Validation;

namespace PartyMaker.Story.WebApplication
{
    public class WebAppGetAuthUserStory : RequestStory<WebAppGetAuthUserStoryContext, WebAppResponseWithEntityDto<WebAppAuthUserResponseDto>>,  IStory<WebAppGetAuthUserStoryContext, WebAppResponseWithEntityDto<WebAppAuthUserResponseDto>>
    {
        private readonly IRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WebAppGetAuthUserStory(
            IRepository repository,
            IWebApproverFactory webApproverFactory,
            IWebAppValidatorFactory webAppValidatorFactory,
            IHttpContextAccessor httpContextAccessor,
            IAppLogger logger)
            :base(webApproverFactory, webAppValidatorFactory, logger)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<WebAppResponseWithEntityDto<WebAppAuthUserResponseDto>> Run(WebAppGetAuthUserStoryContext context)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            var user = await _repository.GetAsync<User>(t => t.Id == Guid.Parse(userId));

            return new WebAppResponseWithEntityDto<WebAppAuthUserResponseDto>
            {
                IsSuccess = true,
                Result = new WebAppAuthUserResponseDto
                {
                    Username = user.Name,
                    UserRole = user.UserRole,
                    Email = user.Email,
                    ImageId = user.ImageId,
                    Phone = user.Phone,
                    Birthday = user.Birthday,
                    Id = user.Id
                },
            };
        }

    }
}
