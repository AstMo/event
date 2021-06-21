using bgTeam;
using bgTeam.DataAccess;
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
    public class CreateEntityStory<TEntity, TStoryContext> : RequestStory<TStoryContext, WebAppResponseDto>
        where TEntity : Entity
        where TStoryContext : CreateEntityStoryContext
    {
        private readonly ITranslatorFactory _translatorFactory;
        private readonly ICrudService _crudService;
        private readonly IAppLogger _appLogger;

        public CreateEntityStory(
            IWebApproverFactory webApproverFactory,
            IWebAppValidatorFactory webAppValidatorFactory,
            ITranslatorFactory translatorFactory,
            ICrudService crudService,
            IAppLogger appLogger)
            :base(webApproverFactory, webAppValidatorFactory, appLogger)
        {
            _translatorFactory = translatorFactory;
            _crudService = crudService;
            _appLogger = appLogger;
        }

        protected override async Task<WebAppResponseDto> Run(TStoryContext context)
        {
            _appLogger.Info($"Start creating new {typeof(TEntity)} story {JsonConvert.SerializeObject(context)}");

            try
            {
                var translator = _translatorFactory.GetTranslator<TStoryContext, TEntity>();
                var entity = translator.Translate(context);
                entity.MarkAsNew();
                var result = await _crudService.InsertAsync(entity);
                await AfterRequest(context, entity.Id);
            }
            catch (Exception ex)
            {
                _appLogger.Error(ex);
                return new WebAppResponseDto { IsSuccess = false, Message = ex.Message, };
            }
            _appLogger.Info($"Success end story creating {typeof(TEntity)}");
            return new WebAppResponseDto { IsSuccess = true, Message = string.Empty, IsTimeout = false };
        }

        protected virtual Task AfterRequest(TStoryContext context, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
