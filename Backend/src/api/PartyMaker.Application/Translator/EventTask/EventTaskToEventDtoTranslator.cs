using AutoMapper;
using bgTeam.DataAccess;
using PartyMaker.Common.Translator;
using PartyMaker.Dto.WebApp.Task;
using System;

namespace PartyMaker.Application.Translator.EventTask
{
    public class EventTaskToEventDtoTranslator : AutomapperTranslator<Domain.Entities.TaskEvent, TaskDto>
    {
        private readonly IRepository _repository;

        public EventTaskToEventDtoTranslator(
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
                .ForMember(t => t.AssignedId, m => m.MapFrom(o => o.AssignedId))
                .ForMember(t => t.Description, m => m.MapFrom(t => t.Description))
                .ForMember(t => t.EventId, m => m.MapFrom(o => o.EventId))
                .ForMember(t => t.Name, m => m.MapFrom(t => t.Name))
                .ForMember(t => t.State, m => m.MapFrom(t => t.State))
                .ForMember(t => t.ExpenseItems, m => m.MapFrom(t => _repository.GetAll<Domain.Entities.ExpenseItem>(r => r.TaskId == t.Id, null, null)));
        }
    }
}
