using bgTeam;
using bgTeam.DataAccess;
using DapperExtensions;
using Microsoft.AspNetCore.Http;
using PartyMaker.Common.Filter;
using PartyMaker.Common.Impl;
using PartyMaker.Common.Sorting;
using PartyMaker.Common.Translator;
using PartyMaker.DataAccess.Common;
using PartyMaker.Dto.WebApp;
using PartyMaker.Dto.WebApp.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PartyMaker.DataAccess.Event
{
    public class WebAppGetEventByPageQuery : GetEntitiesByPageQuery<Domain.Entities.Event, WebAppEventDto, WebAppGetEventByPageQueryContext>,
        IQuery<WebAppGetEventByPageQueryContext, WebAppResponseWithTableDto<WebAppTableDto<WebAppEventDto>, WebAppEventDto>>
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WebAppGetEventByPageQuery(
            IConnectionFactory connectionFactory,
            IHttpContextAccessor httpContextAccessor,
            IRepository repository,
            ITranslatorFactory translatorFactory,
            IAppLogger appLogger)
            : base(repository, translatorFactory, appLogger)
        {
            _connectionFactory = connectionFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<WebAppResponseWithTableDto<WebAppTableDto<WebAppEventDto>, WebAppEventDto>> ExecuteAsync(WebAppGetEventByPageQueryContext context)
        {
            AppLogger.Info($"Start query page {typeof(Domain.Entities.Event)}");
            try
            {
                var sortDir = context.SortDirection == ESortDirection.Asc;
                var sorts = new List<ISort>();
                if (context.SortField != null)
                {
                    sorts.Add(Predicates.Sort(SortingBuilder.Build<Domain.Entities.Event>(context.SortField), sortDir));
                }
                else
                {
                    sorts.Add(Predicates.Sort<Domain.Entities.Event>(x => x.Name, sortDir));
                }

                using var dbConnection = await _connectionFactory.CreateAsync();
                var filterList = context.Filters.ToList();


                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

                var user = await Repository.GetAsync<Domain.Entities.User>(t => t.Id == Guid.Parse(userId), dbConnection);

                var eventIds = (await Repository.GetAllAsync<Domain.Entities.UserEvent>(t => t.UserId == user.Id)).Select(t => t.EventId);
                IList<KeyValuePair<string, IList<string>>> orExpressionValue = new List<KeyValuePair<string, IList<string>>>
                {
                    new KeyValuePair<string, IList<string>>("Id", new List<string>())
                };

                if (!eventIds.Any())
                {
                    return new WebAppResponseWithTableDto<WebAppTableDto<WebAppEventDto>, WebAppEventDto>
                    {
                        Result = new WebAppTableDto<WebAppEventDto>
                        {
                            TotalItems = 0,
                            Items = null,
                        },
                    };
                }

                foreach (var eventId in eventIds)
                {
                    orExpressionValue[0].Value.Add(eventId.ToString());
                }
                var filter = FilterBuilder.Build<Domain.Entities.Event>(filterList.ToArray(), orExpressionValue);

                var pagedResult = await Repository.GetPaginatedResultAsync(filter, sorts, context.Page - 1, context.PageSize, dbConnection);

                var translator = TranslatorFactory.GetTranslator<Domain.Entities.Event, WebAppEventDto>();

                var items = pagedResult.Data.Select(translator.Translate);

                AppLogger.Info($"Finish query {typeof(Domain.Entities.Event)}");
                return new WebAppResponseWithTableDto<WebAppTableDto<WebAppEventDto>, WebAppEventDto>
                {
                    Result = new WebAppTableDto<WebAppEventDto>
                    {
                        TotalItems = pagedResult.Total,
                        Items = items,
                    },
                };
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex);
                return new WebAppResponseWithTableDto<WebAppTableDto<WebAppEventDto>, WebAppEventDto>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
