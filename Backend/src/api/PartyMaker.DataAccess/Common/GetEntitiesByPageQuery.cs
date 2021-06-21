using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Filter;
using PartyMaker.Common.Impl;
using PartyMaker.Common.Sorting;
using PartyMaker.Common.Translator;
using PartyMaker.Domain.Entities;
using PartyMaker.Dto.WebApp;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyMaker.DataAccess.Common
{
    public class GetEntitiesByPageQuery<TEntity, TEntityDto, TEntityContext>
        where TEntity : Entity
        where TEntityDto : WebAppEntityDto
        where TEntityContext : GetEntitiesByPageQueryContext
    {
        protected readonly IRepository Repository;
        protected readonly ITranslatorFactory TranslatorFactory;
        protected readonly IAppLogger AppLogger;

        public GetEntitiesByPageQuery(
            IRepository repository,
            ITranslatorFactory translatorFactory,
            IAppLogger appLogger)
        {
            Repository = repository;
            TranslatorFactory = translatorFactory;
            AppLogger = appLogger;
        }

        public WebAppResponseWithTableDto<WebAppTableDto<TEntityDto>, TEntityDto> Execute(TEntityContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public virtual async Task<WebAppResponseWithTableDto<WebAppTableDto<TEntityDto>, TEntityDto>> ExecuteAsync(TEntityContext context)
        {
            AppLogger.Info($"Start query page {typeof(TEntity)}");
            try
            {
                var sortDir = context.SortDirection == ESortDirection.Asc;
                var sorts = new List<ISort>();
                if (context.SortField != null)
                {
                    sorts.Add(Predicates.Sort(SortingBuilder.Build<TEntity>(context.SortField), sortDir));
                }
                else
                {
                    sorts.Add(Predicates.Sort<TEntity>(x => x.Created, sortDir));
                }

                var filterList = context.Filters.ToList();

                var filter = FilterBuilder.Build<TEntity>(filterList.ToArray());

                var pagedResult = await Repository.GetPaginatedResultAsync(filter, sorts, context.Page - 1, context.PageSize);

                var translator = TranslatorFactory.GetTranslator<TEntity, TEntityDto>();

                var items = pagedResult.Data.Select(translator.Translate);

                AppLogger.Info($"Finish query {typeof(TEntity)}");
                return new WebAppResponseWithTableDto<WebAppTableDto<TEntityDto>, TEntityDto>
                {
                    Result = new WebAppTableDto<TEntityDto>
                    {
                        TotalItems = pagedResult.Total,
                        Items = items,
                    },
                };
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex);
                return new WebAppResponseWithTableDto<WebAppTableDto<TEntityDto>, TEntityDto>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
