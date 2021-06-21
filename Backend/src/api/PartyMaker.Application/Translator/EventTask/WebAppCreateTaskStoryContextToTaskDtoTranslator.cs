using AutoMapper;
using PartyMaker.Common.Translator;
using PartyMaker.Story.WebApplication.Task;
using System;

namespace PartyMaker.Application.Translator.EventTask
{
    public class WebAppCreateTaskStoryContextToTaskDtoTranslator : AutomapperTranslator<WebAppCreateTaskStoryContext, Domain.Entities.TaskEvent>
    {
        public WebAppCreateTaskStoryContextToTaskDtoTranslator(IMapperConfigurationExpression configurationExpression, Lazy<IMapper> mapper)
            : base(configurationExpression, mapper)
        {
        }

        public override void Configure()
        {
            base.Configure();

            Mapping
                .ForMember(t => t.Id, m => m.Ignore())
                .ForMember(t => t.State, m => m.MapFrom(o => o.State))
                .ForMember(t => t.IsDeleted, m => m.Ignore())
                .ForMember(t => t.AssignedId, m => m.MapFrom(t => t.AssignedId))
                .ForMember(t => t.Description, m => m.MapFrom(o => o.Description))
                .ForMember(t => t.Created, m => m.Ignore())
                .ForMember(t => t.Name, m => m.MapFrom(t => t.Name))
                .ForMember(t => t.EventId, m => m.MapFrom(t => t.EventId))
                .ForMember(t => t.Updated, m => m.Ignore());
        }
    }
}
