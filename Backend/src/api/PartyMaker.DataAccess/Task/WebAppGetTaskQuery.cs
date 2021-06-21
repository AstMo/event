using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Translator;
using PartyMaker.DataAccess.Common;
using PartyMaker.Dto.WebApp.Event;

namespace PartyMaker.DataAccess.Task
{
    public class WebAppGetTaskQuery : GetEntityQuery<Domain.Entities.TaskEvent, WebAppEventDto, WebAppGetTaskQueryContext>,
        IQuery<WebAppGetTaskQueryContext, Dto.WebApp.WebAppResponseWithEntityDto<WebAppEventDto>>
    {
        public WebAppGetTaskQuery(
            IRepository repository,
            ITranslatorFactory translatorFactory,
            IAppLogger appLogger)
            :base(repository, appLogger, translatorFactory)
        {

        }
    }
}
