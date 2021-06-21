using AutoMapper;
using PartyMaker.Common.Translator;
using PartyMaker.Dto.WebApp.Localization;
using System;

namespace PartyMaker.Application.Translator.Localization
{
    public class LocalizationItemToLocalizationItemDtoTranslator : AutomapperTranslator<Domain.Entities.LocalizationItem, WebAppLocaleItemDto>
    {
        public LocalizationItemToLocalizationItemDtoTranslator(
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
                .ForMember(t => t.Key, m => m.MapFrom(t => t.Key))
                .ForMember(t => t.Value, m => m.MapFrom(t => t.Value));
        }
    }
}
