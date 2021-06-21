using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Translator;
using PartyMaker.DataAccess.Common;
using PartyMaker.Dto.WebApp.Localization;

namespace PartyMaker.DataAccess.Localization
{
    public class WebAppGetLocaleQuery : GetEntityQuery<Domain.Entities.Localization, WebAppLocaleDto, WebAppGetLocaleQueryContext>,
        IQuery<WebAppGetLocaleQueryContext, Dto.WebApp.WebAppResponseWithEntityDto<WebAppLocaleDto>>
    {
        public WebAppGetLocaleQuery(
            IRepository repository,
            ITranslatorFactory translatorFactory,
            IAppLogger appLogger)
            : base(repository, appLogger, translatorFactory)
        {

        }
    }
}
