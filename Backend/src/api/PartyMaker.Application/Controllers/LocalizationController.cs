using bgTeam;
using bgTeam.DataAccess;
using Microsoft.AspNetCore.Mvc;
using PartyMaker.DataAccess.Localization;
using PartyMaker.Dto.WebApp;
using PartyMaker.Dto.WebApp.Event;
using PartyMaker.Dto.WebApp.Localization;
using PartyMaker.Story.WebApplication.Locales;
using System;
using System.Threading.Tasks;

namespace PartyMaker.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizationController : BaseController
    {
        private readonly IStoryBuilder _storyBuilder;
        private readonly IQueryBuilder _queryBuilder;
        private readonly IAppLogger _appLogger;

        public LocalizationController(IStoryBuilder storyBuilder,
            IQueryBuilder queryBuilder,
            IAppLogger appLogger)
        {
            _storyBuilder = storyBuilder;
            _queryBuilder = queryBuilder;
            _appLogger = appLogger;
        }  

        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(WebAppLocaleTableItemDto), 200)]
        [ProducesResponseType(typeof(WebAppLocaleTableItemDto), 400)]
        public async Task<IActionResult> GetListLocales()
        {
            _appLogger.Info($"Get locales/items request with");
            var result = await _queryBuilder.Build(new WebAppGetLocalesQueryContext() { Page = 1, PageSize = 5000, SortDirection = Common.Impl.ESortDirection.Asc, SortField = null })
                .ReturnAsync<WebAppResponseWithTableDto<WebAppTableDto<WebAppLocaleTableItemDto>, WebAppLocaleTableItemDto>>();

            return GetActionResult(result);
        }

        [HttpGet]
        [Route("item/{id:guid}")]
        [ProducesResponseType(typeof(WebAppLocaleDto), 200)]
        [ProducesResponseType(typeof(WebAppLocaleDto), 400)]
        public async Task<IActionResult> GetLocalization(Guid id)
        {
            _appLogger.Info("Get get locale request");
            var result = await _queryBuilder.Build(new WebAppGetLocaleQueryContext
            {
                Id = id
            }).ReturnAsync<WebAppResponseWithEntityDto<WebAppLocaleDto>>();

            return GetActionResult(result);
        }

        [HttpPost]
        [Route("import/{localeId:guid}/{fileId:guid}")]
        [ProducesResponseType(typeof(WebAppResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppResponseDto), 400)]
        public async Task<IActionResult> ImportLocales(Guid localeId, Guid fileId)
        {
            _appLogger.Info("Get import request locales");
            var result = await _storyBuilder.Build(new WebAppImportLocaleStoryContext()
            {
                LocaleId = localeId,
                FileId = fileId,
            }).ReturnAsync<WebAppResponseDto>();
            return GetActionResult(result);
        }
    }
}
