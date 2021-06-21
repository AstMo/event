using AutoMapper;
using bgTeam.DataAccess;
using PartyMaker.Common.Translator;
using PartyMaker.Domain.Entities;
using PartyMaker.Dto.WebApp.Event;
using System;
using System.Linq;

namespace PartyMaker.Application.Translator.Event
{
    public class EventToEventDtoTranslator : AutomapperTranslator<Domain.Entities.Event, WebAppEventDto>
    {
        private readonly IRepository _repository;

        public EventToEventDtoTranslator(
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
                .ForMember(t => t.Address, m => m.MapFrom(o => o.Address))
                .ForMember(t => t.Date, m => m.MapFrom(t => t.Date))
                .ForMember(t => t.Latitude, m => m.MapFrom(o => o.Latitude))
                .ForMember(t => t.Name, m => m.MapFrom(t => t.Name))
                .ForMember(t => t.Longitude, m => m.MapFrom(t => t.Longitude))
                .ForMember(t => t.TotalBudget, m => m.MapFrom(t => t.TotalBudget))
                .ForMember(t => t.TypeEvent, m => m.MapFrom(t => t.TypeEvent))
                .ForMember(t => t.Participaties, m => m.MapFrom(t => _repository.GetAll<Domain.Entities.UserEvent>(r => r.EventId == t.Id, null, null)));
        }
    }
}
