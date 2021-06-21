using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Translator;
using PartyMaker.Common.Validation;
using PartyMaker.Domain.Entities;
using PartyMaker.Story.CommonStories;
using System.Linq;
using System.Threading.Tasks;

namespace PartyMaker.Story.WebApplication.Event
{
    public class WebAppUpdateEventStory : UpdateEntityStory<Domain.Entities.Event, WebAppUpdateEventStoryContext>, IStory<WebAppUpdateEventStoryContext, PartyMaker.Dto.WebApp.WebAppResponseDto>
    {
        private readonly ICrudService _crudService;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IRepository _repository;

        public WebAppUpdateEventStory(ITranslatorFactory translatorFactory,
            ICrudService crudService,
            IConnectionFactory connectionFactory,
            IRepository repository,
            IWebAppValidatorFactory webAppValidatorFactory,
            IWebApproverFactory webApproverFactory,
            IAppLogger appLogger)
            : base(translatorFactory, repository, crudService,connectionFactory, webAppValidatorFactory, webApproverFactory,  appLogger)
        {
            _crudService = crudService;
            _connectionFactory = connectionFactory;
            _repository = repository;
        }

        protected async override System.Threading.Tasks.Task AfterRequest(WebAppUpdateEventStoryContext context)
        {
            var users = context.Participaties.Select(t => new UserEvent
            {
                UserId = t.UserId.Value,
                EventId = context.Id,
                Role = t.Role
            });

            using var dbConnection = await _connectionFactory.CreateAsync();
            var transaction = dbConnection.BeginTransaction();
            var existUsers = await _repository.GetAllAsync<UserEvent>(t => t.EventId == context.Id);

            foreach (var user in existUsers)
            {
                await _crudService.DeleteAsync(user, dbConnection, transaction);
            }

            foreach (var user in users)
            {
                user.MarkAsNew();
                await _crudService.InsertAsync(user, dbConnection, transaction);
            }
            transaction.Commit();
        }
    }
}
