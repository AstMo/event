using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Common.Translator;
using PartyMaker.Common.Validation;
using PartyMaker.Domain.Entities;
using PartyMaker.Dto.WebApp;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using PartyMaker.Common.Approver;

namespace PartyMaker.Story.CommonStories
{
    public class UpdateEntityStory<TEntity, TStoryContext> : RequestStory<TStoryContext, WebAppResponseDto>
        where TEntity : Entity
        where TStoryContext : UpdateEntityStoryContext
    {
        private readonly ITranslatorFactory _translatorFactory;
        private readonly IRepository _repository;
        private readonly ICrudService _crudService;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IAppLogger _appLogger;

        public UpdateEntityStory(
            ITranslatorFactory translatorFactory,
            IRepository repository,
            ICrudService crudService,
            IConnectionFactory connectionFactory,
            IWebAppValidatorFactory webAppValidatorFactory,
            IWebApproverFactory webApproverFactory,
            IAppLogger appLogger)
            : base(webApproverFactory, webAppValidatorFactory, appLogger)
        {
            _translatorFactory = translatorFactory;
            _repository = repository;
            _crudService = crudService;
            _connectionFactory = connectionFactory;
            _appLogger = appLogger;
        }

        protected override async Task<WebAppResponseDto> Run(TStoryContext context)
        {
            _appLogger.Info($"Start updating {typeof(TEntity)} story ${JsonConvert.SerializeObject(context)}");

            try
            {
                using var dbConnection = await _connectionFactory.CreateAsync();
                var entity = await _repository.GetAsync<TEntity>(t => t.Id == context.Id, dbConnection);

                var translator = _translatorFactory.GetTranslator<TStoryContext, TEntity>();
                translator.Update(context, entity);
                entity.MarkAsUpdated();
                await _crudService.UpdateAsync(entity, dbConnection);

                await AfterRequest(context);
            }
            catch (Exception ex)
            {
                _appLogger.Error(ex);
                return new WebAppResponseDto { IsSuccess = false, Message = ex.Message, };
            }
            _appLogger.Info($"Success end story updating ${typeof(TEntity)}");
            return new WebAppResponseDto { IsSuccess = true, Message = string.Empty, IsTimeout = false };
        }

        protected virtual Task AfterRequest(TStoryContext context)
        {
            return Task.CompletedTask;
        }
    }
}
