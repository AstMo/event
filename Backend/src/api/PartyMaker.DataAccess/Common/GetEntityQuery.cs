using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Translator;
using PartyMaker.Domain.Entities;
using PartyMaker.Dto.WebApp;
using System;
using System.Threading.Tasks;

namespace PartyMaker.DataAccess.Common
{
    public class GetEntityQuery<TEntity, TEntityDto, TEntityContext>
        where TEntity : Entity
        where TEntityDto : WebAppEntityDto
        where TEntityContext : GetEntityQueryContext
    {
        private readonly IRepository _repository;
        private readonly IAppLogger _appLogger;
        private readonly ITranslatorFactory _translatorFactory;

        public GetEntityQuery(
            IRepository repository,
            IAppLogger appLogger,
            ITranslatorFactory translatorFactory)
        {
            _repository = repository;
            _appLogger = appLogger;
            _translatorFactory = translatorFactory;
        }

        public WebAppResponseWithEntityDto<TEntityDto> Execute(TEntityContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<WebAppResponseWithEntityDto<TEntityDto>> ExecuteAsync(TEntityContext context)
        {
            _appLogger.Info($"Start query to get {typeof(TEntity)} with id - {context.Id}");

            try
            {
                var entity = await _repository.GetAsync<TEntity>(t => t.Id == context.Id);

                if (entity == null)
                {
                    _appLogger.Error($"with id  - {context.Id} not found");
                    return new WebAppResponseWithEntityDto<TEntityDto>
                    {
                        IsSuccess = false,
                        IsNotFound = true,
                    };
                }
                var translator = _translatorFactory.GetTranslator<TEntity, TEntityDto>();

                return new WebAppResponseWithEntityDto<TEntityDto>
                {
                    Result = (TEntityDto)translator.Translate(entity),
                };
            }
            catch (Exception ex)
            {
                _appLogger.Error(ex);
                return new WebAppResponseWithEntityDto<TEntityDto>
                {
                    IsSuccess = false,
                };
            }
        }
    }
}
