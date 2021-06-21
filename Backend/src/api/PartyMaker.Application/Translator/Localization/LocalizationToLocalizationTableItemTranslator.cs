using AutoMapper;
using PartyMaker.Common.Translator;
using PartyMaker.Dto.WebApp.Localization;
using System;

namespace PartyMaker.Application.Translator.Localization
{
    public class LocalizationToLocalizationTableItemTranslator : AutomapperTranslator<Domain.Entities.Localization, WebAppLocaleTableItemDto>
    {
        public LocalizationToLocalizationTableItemTranslator(
            IMapperConfigurationExpression configurationExpression,
            Lazy<IMapper> mapper)
            : base(configurationExpression, mapper)
        {
        }

        public override void Configure()
        {
            base.Configure();

            Mapping
                .ForMember(t => t.Id, m => m.MapFrom(t => t.Id))
                .ForMember(t => t.Name, m => m.MapFrom(t => t.Name));
        }
    }
}
