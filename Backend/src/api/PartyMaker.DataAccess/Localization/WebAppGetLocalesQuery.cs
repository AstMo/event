using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Translator;
using PartyMaker.DataAccess.Common;
using PartyMaker.Dto.WebApp;
using PartyMaker.Dto.WebApp.Localization;

namespace PartyMaker.DataAccess.Localization
{
    public class WebAppGetLocalesQuery : GetEntitiesByPageQuery<Domain.Entities.Localization, WebAppLocaleTableItemDto, WebAppGetLocalesQueryContext>,
        IQuery<WebAppGetLocalesQueryContext, WebAppResponseWithTableDto<WebAppTableDto<WebAppLocaleTableItemDto>, WebAppLocaleTableItemDto>>
    {
        public WebAppGetLocalesQuery(
            IRepository repository,
            ITranslatorFactory translatorFactory,
            IAppLogger appLogger)
            : base(repository, translatorFactory, appLogger)
        {

        }
    }
}
