using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Translator;
using PartyMaker.DataAccess.Common;
using PartyMaker.Dto.WebApp.Event;

namespace PartyMaker.DataAccess.Event
{
    public class WebAppGetEventQuery : GetEntityQuery<Domain.Entities.Event, WebAppEventDto, WebAppGetEventQueryContext>,
        IQuery<WebAppGetEventQueryContext, Dto.WebApp.WebAppResponseWithEntityDto<WebAppEventDto>>
    {
        public WebAppGetEventQuery(
            IRepository repository,
            ITranslatorFactory translatorFactory,
            IAppLogger appLogger)
            :base(repository, appLogger, translatorFactory)
        {

        }
    }
}
