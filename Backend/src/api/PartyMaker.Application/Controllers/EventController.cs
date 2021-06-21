using bgTeam;
using bgTeam.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PartyMaker.Common.Email;
using PartyMaker.DataAccess.Event;
using PartyMaker.Dto.WebApp;
using PartyMaker.Dto.WebApp.Event;
using PartyMaker.Story.WebApplication.Account;
using PartyMaker.Story.WebApplication.Event;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PartyMaker.Application.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : BaseController
    {
        private readonly IWebAppEmailSenderService _webAppEmailSenderService;
        private readonly IStoryBuilder _storyBuilder;
        private readonly IQueryBuilder _queryBuilder;
        private readonly IAppLogger _appLogger;

        public EventController(
            IWebAppEmailSenderService webAppEmailSenderService,
            IStoryBuilder storyBuilder,
            IQueryBuilder queryBuilder,
            IAppLogger appLogger)
        {
            _webAppEmailSenderService = webAppEmailSenderService;
            _storyBuilder = storyBuilder;
            _queryBuilder = queryBuilder;
            _appLogger = appLogger;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(WebAppResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppResponseDto), 400)]
        public async Task<IActionResult> CreateEvent(WebAppEventDto createEvent)
        {
            _appLogger.Info("Get request to save event");
            var result = await _storyBuilder.Build(new WebAppCreateEventStoryContext {
                Address = createEvent.Address,
                Date = createEvent.Date,
                Latitude = createEvent.Latitude,
                Longitude = createEvent.Longitude,
                Name = createEvent.Name,
                Participaties = createEvent.Participaties,
                TotalBudget = createEvent.TotalBudget,
                TypeEvent = createEvent.TypeEvent
            }).ReturnAsync<WebAppResponseDto>();

            return GetActionResult(result);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ProducesResponseType(typeof(WebAppResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppResponseDto), 400)]
        public async Task<IActionResult> UpdateEvent([FromBody] WebAppEventDto createEvent, Guid id)
        {
            _appLogger.Info("Get request to update event");
            var result = await _storyBuilder.Build(new WebAppUpdateEventStoryContext
            {
                Id = id,
                Address = createEvent.Address,
                Date = createEvent.Date,
                Latitude = createEvent.Latitude,
                Longitude = createEvent.Longitude,
                Name = createEvent.Name,
                Participaties = createEvent.Participaties,
                TotalBudget = createEvent.TotalBudget,
                TypeEvent = createEvent.TypeEvent
            }).ReturnAsync<WebAppResponseDto>();

            return GetActionResult(result);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [ProducesResponseType(typeof(WebAppResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppResponseDto), 400)]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            _appLogger.Info("Get delete event request");
            var result = await _storyBuilder.Build(new WebAppDeleteEventStoryContext
            {
                Id = id
            }).ReturnAsync<WebAppResponseDto>();

            return GetActionResult(result);
        }

        [HttpGet]
        [Route("item/{id:guid}")]
        [ProducesResponseType(typeof(WebAppEventDto), 200)]
        [ProducesResponseType(typeof(WebAppEventDto), 400)]
        public async Task<IActionResult> GetEvent(Guid id)
        {
            _appLogger.Info("Get get event request");
            var result = await _queryBuilder.Build(new WebAppGetEventQueryContext
            {
                Id = id
            }).ReturnAsync<WebAppResponseWithEntityDto<WebAppEventDto>>();

            return GetActionResult(result);
        }

        [HttpPost]
        [Route("invite")]
        [ProducesResponseType(typeof(WebAppResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppResponseDto), 400)]
        public async Task<IActionResult> InvitePesonByEmail([FromBody] InvitePersonDto inviteDto)
        {
            _appLogger.Info("Invite person by email request");
            var result = await _storyBuilder.Build(new WebAppInvitePersonByEmailStoryContext
            {
                Email = inviteDto.Email,
            }).ReturnAsync<WebAppResponseDto>();

            var actionResult = GetActionResult(result);
            if (result.IsSuccess)
            {
                var isOk = _webAppEmailSenderService.SendInviteUserInfo(inviteDto.Email, inviteDto.EventName);
                if (!isOk)
                {
                    return StatusCode((int)HttpStatusCode.Conflict, result);
                }
            }
            return actionResult;
        }

        [HttpGet]
        [Route("items/{page}/{pageSize?}")]
        [ProducesResponseType(typeof(WebAppEventDto), 200)]
        [ProducesResponseType(typeof(WebAppEventDto), 400)]
        public async Task<IActionResult> GetTable(int page, int pageSize = 50)
        {
            _appLogger.Info($"Get event/items request with {page} and {pageSize}. Getting filter and sort params");

            page = Convert.ToInt32(Request.Query["pageNumber"].FirstOrDefault() ?? page.ToString());
            pageSize = Convert.ToInt32(Request.Query["pageSize"].FirstOrDefault() ?? pageSize.ToString());

            var filters = this.CreateFilters();
            var sortBy = this.GetSortField();
            var sortDir = this.GetSortDirection();

            _appLogger.Info($"Get params filter {JsonConvert.SerializeObject(filters)} and sort {sortBy} - {sortDir} for event/items request");

            var result = await _queryBuilder.Build(new WebAppGetEventByPageQueryContext() { Page = page, PageSize = pageSize, Filters = filters, SortDirection = sortDir, SortField = sortBy })
                .ReturnAsync<WebAppResponseWithTableDto<WebAppTableDto<WebAppEventDto>, WebAppEventDto>>();

            return GetActionResult(result);
        }
    }
}
