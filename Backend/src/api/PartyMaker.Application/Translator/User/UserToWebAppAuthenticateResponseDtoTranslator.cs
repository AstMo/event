using AutoMapper;
using PartyMaker.Common.Translator;
using PartyMaker.Dto.WebApp;
using System;

namespace PartyMaker.Application.Translator
{
    public class UserToWebAppAuthenticateResponseDtoTranslator : AutomapperTranslator<Domain.Entities.User, WebAppAuthenticateResponceDto>
    {
        public UserToWebAppAuthenticateResponseDtoTranslator(IMapperConfigurationExpression configurationExpression, Lazy<IMapper> mapper)
            : base(configurationExpression, mapper)
        {
        }

        public override void Configure()
        {
            base.Configure();

            Mapping
                .ForMember(t => t.Id, m => m.MapFrom(o => o.Id))
                .ForMember(t => t.Username, m => m.MapFrom(o => o.Name))
                .ForMember(t => t.WrongCredentials, m => m.Ignore())
                .ForMember(t => t.UserRole, m => m.MapFrom(o => o.UserRole))
                .ForMember(t => t.Token, m => m.Ignore())
                .ForMember(t => t.ExpiresTime, m => m.Ignore())
                .ForMember(t => t.RefreshToken, m => m.Ignore());
        }
    }
}
