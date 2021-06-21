using bgTeam;
using bgTeam.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PartyMaker.Common.Email;
using PartyMaker.DataAccess.Event;
using PartyMaker.DataAccess.Task;
using PartyMaker.Dto.WebApp;
using PartyMaker.Dto.WebApp.Task;
using PartyMaker.Story.WebApplication.Task;
using System;
using System.Threading.Tasks;

namespace PartyMaker.Application.Controllers
{
    [Authorize]
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class TaskController : BaseController
    {
        private readonly IStoryBuilder _storyBuilder;
        private readonly IQueryBuilder _queryBuilder;
        private readonly IWebAppEmailSenderService _webAppEmailSenderService;
        private readonly IAppLogger _appLogger;

        public TaskController(IStoryBuilder storyBuilder,
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
        [Microsoft.AspNetCore.Mvc.Route("create")]
        [ProducesResponseType(typeof(WebAppResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppResponseDto), 400)]
        public async Task<IActionResult> CreateTask(TaskDto createEvent)
        {
            _appLogger.Info("Get request to save task");
            var result = await _storyBuilder.Build(new WebAppCreateTaskStoryContext
            {
                State = createEvent.State,
                AssignedId = createEvent.AssignedId,
                Description = createEvent.Description,
                EventId = createEvent.EventId,
                Name = createEvent.Name,
            }
            ).ReturnAsync<WebAppResponseDto>();

            return GetActionResult(result);
        }

        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("update/{id:guid}")]
        [ProducesResponseType(typeof(WebAppResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppResponseDto), 400)]
        public async Task<IActionResult> UpdateTask([FromBody] TaskDto updateEvent, Guid id)
        {
            _appLogger.Info("Get request to update task");
            var result = await _storyBuilder.Build(new WebAppUpdateTaskStoryContext {
                State = updateEvent.State,
                AssignedId = updateEvent.AssignedId,
                Description = updateEvent.Description,
                EventId = updateEvent.EventId,
                Name = updateEvent.Name,
                Id = updateEvent.Id.Value,
            }).ReturnAsync<WebAppResponseDto>();

            return GetActionResult(result);
        }

        [HttpDelete]
        [Microsoft.AspNetCore.Mvc.Route("{id:guid}")]
        [ProducesResponseType(typeof(WebAppResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppResponseDto), 400)]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            _appLogger.Info("Get delete task request");
            var result = await _storyBuilder.Build(new WebAppDeleteTaskStoryContext
            {
                Id = id
            }).ReturnAsync<WebAppResponseDto>();

            return GetActionResult(result);
        }

        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("{id:guid}")]
        [ProducesResponseType(typeof(TaskDto), 200)]
        [ProducesResponseType(typeof(TaskDto), 400)]
        public async Task<IActionResult> GetTask(Guid id)
        {
            _appLogger.Info("Get get task request");
            var result = await _queryBuilder.Build(new WebAppGetEventQueryContext
            {
                Id = id
            }).ReturnAsync<WebAppResponseWithEntityDto<TaskDto>>();

            return GetActionResult(result);
        }


        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("items/{page}/{pageSize?}")]
        [ProducesResponseType(typeof(TaskDto), 200)]
        [ProducesResponseType(typeof(TaskDto), 400)]
        public async Task<IActionResult> GetTableTask(int page, int pageSize = 50)
        {
            _appLogger.Info($"Get task/items request with {page} and {pageSize}. Getting filter and sort params");

            var filters = this.CreateFilters();
            var sortBy = this.GetSortField();
            var sortDir = this.GetSortDirection();

            _appLogger.Info($"Get params filter {JsonConvert.SerializeObject(filters)} and sort {sortBy} - {sortDir} for task/items request");

            var result = await _queryBuilder.Build(new WebAppGetTaskByPageQueryContext() { Page = page, PageSize = pageSize, Filters = filters, SortDirection = sortDir, SortField = sortBy })
                .ReturnAsync<WebAppResponseWithTableDto<WebAppTableDto<TaskDto>, TaskDto>>();

            return GetActionResult(result);
        }
    }
}
