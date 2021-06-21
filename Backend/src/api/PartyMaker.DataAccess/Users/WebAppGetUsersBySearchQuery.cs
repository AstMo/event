using bgTeam;
using bgTeam.DataAccess;
using DapperExtensions;
using PartyMaker.Common.Filter;
using PartyMaker.Common.Translator;
using PartyMaker.DataAccess.Common;
using PartyMaker.Dto.WebApp;
using PartyMaker.Dto.WebApp.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartyMaker.DataAccess.Users
{
    public class WebAppGetUsersBySearchQuery : IQuery<WebAppGetUsersBySearchQueryContext, WebAppResponseWithTableDto<WebAppTableDto<WebAppUserTableItemDto>, WebAppUserTableItemDto>>
    {
        private readonly IRepository _repository;
        private readonly ITranslatorFactory _translatorFactory;
        private readonly IAppLogger _appLogger;

        public WebAppGetUsersBySearchQuery(
            IRepository repository,
            ITranslatorFactory translatorFactory,
            IAppLogger appLogger)
        {
            _repository = repository;
            _translatorFactory = translatorFactory;
            _appLogger = appLogger;
        }

        public WebAppResponseWithTableDto<WebAppTableDto<WebAppUserTableItemDto>, WebAppUserTableItemDto> Execute(WebAppGetUsersBySearchQueryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<WebAppResponseWithTableDto<WebAppTableDto<WebAppUserTableItemDto>, WebAppUserTableItemDto>> ExecuteAsync(WebAppGetUsersBySearchQueryContext context)
        {
            _appLogger.Info($"Start search query {typeof(Domain.Entities.User)}");
            try
            {
                var sorts = new List<ISort>();
                sorts.Add(Predicates.Sort<Domain.Entities.User>(x => x.Name));

                var filterList = new List<KeyValuePair<string, string>>();
                IList<KeyValuePair<string, IList<string>>> orExpressionValue = new List<KeyValuePair<string, IList<string>>>
                {
                    new KeyValuePair<string, IList<string>>("Name", new List<string>() { context.SearchQuery }),
                    new KeyValuePair<string, IList<string>>("Email", new List<string>() { context.SearchQuery })
                };

                var filter = FilterBuilder.Build<Domain.Entities.User>(filterList.ToArray(), orExpressionValue);

                var pagedResult = await _repository.GetPaginatedResultAsync(filter, sorts, 0, 20);

                var translator = _translatorFactory.GetTranslator<Domain.Entities.User, WebAppUserTableItemDto>();

                var items = pagedResult.Data.Select(translator.Translate);
                _appLogger.Info($"Finish query {typeof(Domain.Entities.User)}");
                return new WebAppResponseWithTableDto<WebAppTableDto<WebAppUserTableItemDto>, WebAppUserTableItemDto>
                {
                    Result = new WebAppTableDto<WebAppUserTableItemDto>
                    {
                        TotalItems = pagedResult.Total,
                        Items = items,
                    },
                };
            }
            catch (Exception ex)
            {
                _appLogger.Error(ex);
                return new WebAppResponseWithTableDto<WebAppTableDto<WebAppUserTableItemDto>, WebAppUserTableItemDto>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
