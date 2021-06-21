using AutoMapper;
using PartyMaker.Common.Translator;
using PartyMaker.Story.WebApplication.Event;
using System;

namespace PartyMaker.Application.Translator.Event
{
    public class WebAppCreateEventStoryContextToEventTranslator : AutomapperTranslator<WebAppCreateEventStoryContext, Domain.Entities.Event>
    {
        public WebAppCreateEventStoryContextToEventTranslator(IMapperConfigurationExpression configurationExpression, Lazy<IMapper> mapper)
            : base(configurationExpression, mapper)
        {
        }

        public override void Configure()
        {
            base.Configure();

            Mapping
                .ForMember(t => t.Id, m => m.Ignore())
                .ForMember(t => t.Address, m => m.MapFrom(o => o.Address))
                .ForMember(t => t.IsDeleted, m => m.Ignore())
                .ForMember(t => t.Date, m => m.MapFrom(t => t.Date))
                .ForMember(t => t.Latitude, m => m.MapFrom(o => o.Latitude))
                .ForMember(t => t.Created, m => m.Ignore())
                .ForMember(t => t.Name, m => m.MapFrom(t => t.Name))
                .ForMember(t => t.Longitude, m => m.MapFrom(t => t.Longitude))
                .ForMember(t => t.TotalBudget, m => m.MapFrom(t => t.TotalBudget))
                .ForMember(t => t.TypeEvent, m => m.MapFrom(t => t.TypeEvent))
                .ForMember(t => t.Updated, m => m.Ignore());
        }
    }
}
