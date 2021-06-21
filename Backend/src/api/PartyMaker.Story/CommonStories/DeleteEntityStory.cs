using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Common.Validation;
using PartyMaker.Domain.Entities;
using PartyMaker.Dto.WebApp;
using System;
using System.Threading.Tasks;

namespace PartyMaker.Story.CommonStories
{
    public class DeleteEntityStory<TEntity, TStoryContext> : RequestStory<TStoryContext, WebAppResponseDto>
       where TEntity : Entity
       where TStoryContext : DeleteEntityStoryContext
    {
        private readonly IRepository _repository;
        private readonly ICrudService _crudService;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IAppLogger _appLogger;

        public DeleteEntityStory(
            IWebApproverFactory webApproverFactory,
            IWebAppValidatorFactory webAppValidatorFactory,
            IRepository repository,
            ICrudService crudService,
            IConnectionFactory connectionFactory,
            IAppLogger appLogger)
            : base(webApproverFactory, webAppValidatorFactory, appLogger)
        {
            _repository = repository;
            _crudService = crudService;
            _connectionFactory = connectionFactory;
            _appLogger = appLogger;
        }

        protected override async Task<WebAppResponseDto> Run(TStoryContext context)
        {
            _appLogger.Info($"Start deleting new {typeof(TEntity)} story ${context.Id}");
            try
            {
                using var dbConnection = await _connectionFactory.CreateAsync();
                var entity = await _repository.GetAsync<TEntity>(t => t.Id == context.Id, dbConnection);

                entity.MarkAsDeleted();
                await _crudService.UpdateAsync(entity, dbConnection);
                await AfterRequest(context);
            }
            catch (Exception ex)
            {
                _appLogger.Error(ex);
                return new WebAppResponseDto { IsSuccess = false, Message = ex.Message, };
            }
            _appLogger.Info($"Success end story deleting {typeof(TEntity)}");
            return new WebAppResponseDto { IsSuccess = true, Message = string.Empty, IsTimeout = false };
        }

        protected virtual Task AfterRequest(TStoryContext context)
        {
            throw new NotImplementedException();
        }
    }
}
