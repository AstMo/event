using bgTeam;
using bgTeam.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PartyMaker.Common.Email;
using PartyMaker.DataAccess.ExpenseItem;
using PartyMaker.Dto.WebApp;
using PartyMaker.Dto.WebApp.ExpenseItem;
using PartyMaker.Dto.WebApp.ExpensesItem;
using PartyMaker.Story.WebApplication.ExpenseItem;
using System;
using System.Threading.Tasks;

namespace PartyMaker.Application.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseItemController : BaseController
    {
        private readonly IWebAppEmailSenderService _webAppEmailSenderService;
        private readonly IStoryBuilder _storyBuilder;
        private readonly IQueryBuilder _queryBuilder;
        private readonly IAppLogger _appLogger;

        public ExpenseItemController(
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
        public async Task<IActionResult> CreateExpenceItem(ExpenseItemDto createEvent)
        {
            _appLogger.Info("Get request to save event");
            var result = await _storyBuilder.Build(new WebAppCreateExpenseItemStoryContext
            {
            }).ReturnAsync<WebAppResponseDto>();

            return GetActionResult(result);
        }

        [HttpPost]
        [Route("update/{id:guid}")]
        [ProducesResponseType(typeof(WebAppResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppResponseDto), 400)]
        public async Task<IActionResult> UpdateExpenceItem([FromBody] ExpenseItemDto createEvent, Guid id)
        {
            _appLogger.Info("Get request to update event");
            var result = await _storyBuilder.Build(new WebAppUpdateExpenseItemStoryContext
            {
            }).ReturnAsync<WebAppResponseDto>();

            return GetActionResult(result);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [ProducesResponseType(typeof(WebAppResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppResponseDto), 400)]
        public async Task<IActionResult> DeleteExpenceItem(Guid id)
        {
            _appLogger.Info("Get delete event request");
            var result = await _storyBuilder.Build(new WebAppDeleteExpenseItemStoryContext
            {
                Id = id
            }).ReturnAsync<WebAppResponseDto>();

            return GetActionResult(result);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ProducesResponseType(typeof(ExpenseItemDto), 200)]
        [ProducesResponseType(typeof(ExpenseItemDto), 400)]
        public async Task<IActionResult> GetEvent(Guid id)
        {
            _appLogger.Info("Get get event request");
            var result = await _queryBuilder.Build(new WebAppGetExpenseItemQueryContext
            {
                Id = id
            }).ReturnAsync<WebAppResponseWithEntityDto<ExpenseItemDto>>();

            return GetActionResult(result);
        }


        [HttpGet]
        [Route("items/{page}/{pageSize?}")]
        [ProducesResponseType(typeof(ExpenseItemDto), 200)]
        [ProducesResponseType(typeof(ExpenseItemDto), 400)]
        public async Task<IActionResult> GetTableExpenceItem(int page, int pageSize = 50)
        {
            _appLogger.Info($"Get event/items request with {page} and {pageSize}. Getting filter and sort params");

            var filters = this.CreateFilters();
            var sortBy = this.GetSortField();
            var sortDir = this.GetSortDirection();

            _appLogger.Info($"Get params filter {JsonConvert.SerializeObject(filters)} and sort {sortBy} - {sortDir} for event/items request");

            var result = await _queryBuilder.Build(new WebAppGetExpenseItemByPageQueryContext() { Page = page, PageSize = pageSize, Filters = filters, SortDirection = sortDir, SortField = sortBy })
                .ReturnAsync<WebAppResponseWithTableDto<WebAppTableDto<ExpenseItemDto>, ExpenseItemDto>>();

            return GetActionResult(result);
        }
    }
}
