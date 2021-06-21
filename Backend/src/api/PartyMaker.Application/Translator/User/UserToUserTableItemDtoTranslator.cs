using AutoMapper;
using PartyMaker.Common.Translator;
using PartyMaker.Dto.WebApp.Users;
using System;

namespace PartyMaker.Application.Translator.User
{
    public class UserToUserTableItemDtoTranslator : AutomapperTranslator<Domain.Entities.User, WebAppUserTableItemDto>
    {
        public UserToUserTableItemDtoTranslator(IMapperConfigurationExpression configurationExpression, Lazy<IMapper> mapper)
            : base(configurationExpression, mapper)
        {
        }

        public override void Configure()
        {
            base.Configure();

            Mapping
                .ForMember(t => t.Id, m => m.MapFrom(o => o.Id))
                .ForMember(t => t.Name, m => m.MapFrom(o => o.Name))
                .ForMember(t => t.Email, m => m.MapFrom(o => o.Email))
                .ForMember(t => t.Birthday, m => m.MapFrom(o => o.Birthday));
        }
    }
}
