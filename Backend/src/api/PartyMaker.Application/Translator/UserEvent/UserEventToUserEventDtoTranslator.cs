using AutoMapper;
using bgTeam.DataAccess;
using PartyMaker.Common.Translator;
using PartyMaker.Dto.WebApp.Event;
using System;

namespace PartyMaker.Application.Translator.UserEvent
{
    public class UserEventToUserEventDtoTranslator : AutomapperTranslator<Domain.Entities.UserEvent, WebAppUserEventDto>
    {
        private readonly IRepository _repository;

        public UserEventToUserEventDtoTranslator(
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
                .ForMember(t => t.UserId, m => m.MapFrom(t => t.UserId))
                .ForMember(t => t.Role, m => m.MapFrom(o => o.Role))
                .ForMember(t => t.Name, m => m.MapFrom(t => _repository.Get<Domain.Entities.User>(r => t.UserId == r.Id, null, null).Name));
        }
    }
}
