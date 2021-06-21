using AutoMapper;
using PartyMaker.Common.Translator;
using PartyMaker.Story.WebApplication.ExpenseItem;
using System;

namespace PartyMaker.Application.Translator.ExpenseItem
{
    public class WebAppExpenseItemCreateStoryContextToExpenseItemTranslator : AutomapperTranslator<WebAppCreateExpenseItemStoryContext, Domain.Entities.ExpenseItem>
    {
        public WebAppExpenseItemCreateStoryContextToExpenseItemTranslator(IMapperConfigurationExpression configurationExpression, Lazy<IMapper> mapper)
            : base(configurationExpression, mapper)
        {
        }

        public override void Configure()
        {
            base.Configure();

            Mapping
                .ForMember(t => t.Id, m => m.Ignore())
                .ForMember(t => t.AssignedId, m => m.MapFrom(o => o.AssignedId))
                .ForMember(t => t.IsDeleted, m => m.Ignore())
                .ForMember(t => t.Description, m => m.MapFrom(t => t.Description))
                .ForMember(t => t.EventId, m => m.MapFrom(o => o.EventId))
                .ForMember(t => t.Created, m => m.Ignore())
                .ForMember(t => t.Name, m => m.MapFrom(t => t.Name))
                .ForMember(t => t.Price, m => m.MapFrom(t => t.Price))
                .ForMember(t => t.TaskId, m => m.MapFrom(t => t.TaskId))
                .ForMember(t => t.Updated, m => m.Ignore());
        }
    }
}
