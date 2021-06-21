using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Translator;
using PartyMaker.Common.Validation;
using PartyMaker.Story.CommonStories;

namespace PartyMaker.Story.WebApplication.Event
{
    public class WebAppDeleteEventStory : DeleteEntityStory<Domain.Entities.Event, WebAppDeleteEventStoryContext>, IStory<WebAppDeleteEventStoryContext, PartyMaker.Dto.WebApp.WebAppResponseDto>
    {

        public WebAppDeleteEventStory(ITranslatorFactory translatorFactory,
            ICrudService crudService,
            IRepository repository,
            IConnectionFactory connectionFactory,
            IWebAppValidatorFactory webAppValidatorFactory,
            IWebApproverFactory webApproverFactory,
            IAppLogger appLogger)
            : base(webApproverFactory, webAppValidatorFactory, repository, crudService, connectionFactory, appLogger)
        {
        }
    }
}
