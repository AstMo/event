using AutoMapper;
using bgTeam.DataAccess;
using PartyMaker.Common.Translator;
using PartyMaker.Dto.WebApp.ExpensesItem;
using System;

namespace PartyMaker.Application.Translator.ExpenseItem
{
    public class ExpenceItemToExpenseItemDtoTranslator : AutomapperTranslator<Domain.Entities.ExpenseItem, ExpenseItemDto>
    {
        private readonly IRepository _repository;

        public ExpenceItemToExpenseItemDtoTranslator(
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
                .ForMember(t => t.EventId, m => m.MapFrom(t => t.EventId))
                .ForMember(t => t.Name, m => m.MapFrom(t => t.Name))
                .ForMember(t => t.Price, m => m.MapFrom(t => t.Price))
                .ForMember(t => t.TaskId, m => m.MapFrom(t => t.TaskId));
        }
    }
}
