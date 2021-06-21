using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Validation;
using PartyMaker.Story.CommonStories;

namespace PartyMaker.Story.WebApplication.Task
{
    public class WebAppDeleteTaskStory : DeleteEntityStory<Domain.Entities.TaskEvent, WebAppDeleteTaskStoryContext>, IStory<WebAppDeleteTaskStoryContext, PartyMaker.Dto.WebApp.WebAppResponseDto>
    {

        public WebAppDeleteTaskStory(
            ICrudService crudService,
            IRepository repository,
            IConnectionFactory connectionFactory,
            IWebAppValidatorFactory webAppValidatorFactory,
            IWebApproverFactory webApproverFactory,
            IAppLogger appLogger)
            : base(webApproverFactory, webAppValidatorFactory, repository, crudService, connectionFactory, appLogger)
        {
        }

        protected async override System.Threading.Tasks.Task AfterRequest(WebAppDeleteTaskStoryContext context)
        {

        }
    }
}
