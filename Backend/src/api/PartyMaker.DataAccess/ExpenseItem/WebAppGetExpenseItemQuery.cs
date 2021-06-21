using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Translator;
using PartyMaker.DataAccess.Common;
using PartyMaker.Dto.WebApp.Event;

namespace PartyMaker.DataAccess.ExpenseItem
{
    public class WebAppGetExpenseItemQuery : GetEntityQuery<Domain.Entities.ExpenseItem, WebAppEventDto, WebAppGetExpenseItemQueryContext>,
        IQuery<WebAppGetExpenseItemQueryContext, Dto.WebApp.WebAppResponseWithEntityDto<WebAppEventDto>>
    {
        public WebAppGetExpenseItemQuery(
            IRepository repository,
            ITranslatorFactory translatorFactory,
            IAppLogger appLogger)
            :base(repository, appLogger, translatorFactory)
        {

        }
    }
}
