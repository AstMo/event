using AutoMapper;
using bgTeam.DataAccess;
using PartyMaker.Common.Translator;
using PartyMaker.Dto.WebApp.Localization;
using System;

namespace PartyMaker.Application.Translator.Localization
{
    public class LocalizationToLocalizationItemTranslator : AutomapperTranslator<Domain.Entities.Localization, WebAppLocaleDto>
    {
        private readonly IRepository _repository;

        public LocalizationToLocalizationItemTranslator(
            IRepository repository,
            IMapperConfigurationExpression configurationExpression,
            Lazy<IMapper> mapper)
            : base(configurationExpression, mapper)
        {
            _repository = repository;
        }

        public override void Configure()
        {
            base.Configure();

            Mapping
                .ForMember(t => t.Id, m => m.MapFrom(t => t.Id))
                .ForMember(t => t.Name, m => m.MapFrom(o => o.Name))
                .ForMember(t => t.Items, m => m.MapFrom(t => _repository.GetAll<Domain.Entities.LocalizationItem>(r => r.LocalizationId == t.Id, null, null)));
        }
    }
}
