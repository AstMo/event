using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Translator;
using PartyMaker.Common.Validation;
using PartyMaker.Domain.Entities;
using PartyMaker.Story.CommonStories;

namespace PartyMaker.Story.WebApplication.Account
{
    public class WebAppUpdateUserStory : UpdateEntityStory<User, WebAppUpdateUserStoryContext>, IStory<WebAppUpdateUserStoryContext, PartyMaker.Dto.WebApp.WebAppResponseDto>
    {
        public WebAppUpdateUserStory(ITranslatorFactory translatorFactory,
            IRepository repository,
            ICrudService crudService,
            IConnectionFactory connectionFactory,
            IWebAppValidatorFactory webAppValidatorFactory,
            IWebApproverFactory webApproverFactory,
            IAppLogger appLogger) 
            : base(translatorFactory, repository, crudService, connectionFactory, webAppValidatorFactory, webApproverFactory, appLogger)
        {
        }
    }
}
