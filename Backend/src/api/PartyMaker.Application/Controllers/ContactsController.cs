using bgTeam;
using bgTeam.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PartyMaker.Dto.WebApp;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PartyMaker.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : BaseController
    {
        private readonly IStoryBuilder _storyBuilder;
        private readonly IQueryBuilder _queryBuilder;
        private readonly IAppLogger _appLogger;

        public ContactsController(
            IStoryBuilder storyBuilder,
            IQueryBuilder queryBuilder,
            IAppLogger appLogger)
        {
            _storyBuilder = storyBuilder;
            _queryBuilder = queryBuilder;
            _appLogger = appLogger;
        }

        [HttpGet]
        [Route("items/{page}/{pageSize?}")]
        public async Task<IActionResult> GetTable(int page, int pageSize = 50)
        {
            throw new NotImplementedException();
           /* _appLogger.Info($"Get contacts/items request with {page} and {pageSize}. Getting filter and sort params");

            page = Convert.ToInt32(Request.Query["pageNumber"].FirstOrDefault() ?? page.ToString());
            pageSize = Convert.ToInt32(Request.Query["pageSize"].FirstOrDefault() ?? pageSize.ToString());

            var filters = this.CreateFilters();
            var sortBy = this.GetSortField();
            var sortDir = this.GetSortDirection();

            _appLogger.Info($"Get params filter {JsonConvert.SerializeObject(filters)} and sort {sortBy} - {sortDir} for event/items request");

            var result = await _queryBuilder.Build(new WebAppGetContactsByPageQueryContext() { Page = page, PageSize = pageSize, Filters = filters, SortDirection = sortDir, SortField = sortBy })
                .ReturnAsync<WebAppResponseWithTableDto<WebAppTableDto<WebAppContactsDto>, WebAppContactsDto>>();

            return GetActionResult(result);*/
        }
    }
}
