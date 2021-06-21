using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Translator;
using PartyMaker.Common.Validation;
using PartyMaker.Domain.Entities;
using PartyMaker.Story.CommonStories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PartyMaker.Story.WebApplication.Event
{
    public class WebAppCreateEventStory : CreateEntityStory<Domain.Entities.Event, WebAppCreateEventStoryContext>, IStory<WebAppCreateEventStoryContext, PartyMaker.Dto.WebApp.WebAppResponseDto>
    {
        private readonly ITranslatorFactory _translatorFactory;
        private readonly ICrudService _crudService;
        private readonly IConnectionFactory _connectionFactory;

        public WebAppCreateEventStory(ITranslatorFactory translatorFactory,
            ICrudService crudService,
            IConnectionFactory connectionFactory,
            IWebAppValidatorFactory webAppValidatorFactory,
            IWebApproverFactory webApproverFactory,
            IAppLogger appLogger)
            : base(webApproverFactory, webAppValidatorFactory, translatorFactory, crudService, appLogger)
        {
            _translatorFactory = translatorFactory;
            _crudService = crudService;
            _connectionFactory = connectionFactory;
        }

        protected async override System.Threading.Tasks.Task AfterRequest(WebAppCreateEventStoryContext context, Guid id)
        {
            var users = context.Participaties.Select(t => new UserEvent
            {
                UserId = t.UserId.Value,
                EventId = id,
                Role = t.Role
            });

            using var dbConnection = await _connectionFactory.CreateAsync();
            var transaction = dbConnection.BeginTransaction();
            foreach(var user in users)
            {
                user.MarkAsNew();
                await _crudService.InsertAsync(user, dbConnection, transaction);
            }
            transaction.Commit();
        }
    }
}
