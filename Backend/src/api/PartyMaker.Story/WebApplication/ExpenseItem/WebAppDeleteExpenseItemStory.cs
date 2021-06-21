using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Validation;
using PartyMaker.Story.CommonStories;

namespace PartyMaker.Story.WebApplication.ExpenseItem
{
    public class WebAppDeleteExpenseItemStory : DeleteEntityStory<Domain.Entities.ExpenseItem, WebAppDeleteExpenseItemStoryContext>, IStory<WebAppDeleteExpenseItemStoryContext, PartyMaker.Dto.WebApp.WebAppResponseDto>
    {

        public WebAppDeleteExpenseItemStory(
            ICrudService crudService,
            IRepository repository,
            IConnectionFactory connectionFactory,
            IWebAppValidatorFactory webAppValidatorFactory,
            IWebApproverFactory webApproverFactory,
            IAppLogger appLogger)
            : base(webApproverFactory, webAppValidatorFactory, repository, crudService, connectionFactory, appLogger)
        {
        }

        protected async override System.Threading.Tasks.Task AfterRequest(WebAppDeleteExpenseItemStoryContext context)
        {

        }
    }
}
