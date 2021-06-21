using AutoMapper;
using PartyMaker.Common.Translator;
using PartyMaker.Domain.Entities;
using PartyMaker.Story.WebApplication.Account;
using System;

namespace PartyMaker.Application.Translator.User
{
    public class WebAppUpdateUserStoryContextToUserTranslator : AutomapperTranslator<WebAppUpdateUserStoryContext, Domain.Entities.User>
    {
        public WebAppUpdateUserStoryContextToUserTranslator(IMapperConfigurationExpression configurationExpression, Lazy<IMapper> mapper)
            : base(configurationExpression, mapper)
        {
        }

        public override void Configure()
        {
            base.Configure();

            Mapping
                .ForMember(t => t.Id, m => m.Ignore())
                .ForMember(t => t.ImageId, m => m.MapFrom(o => o.ImageId))
                .ForMember(t => t.IsDeleted, m => m.Ignore())
                .ForMember(t => t.UserRole, m => m.MapFrom(t => EUserRole.PartyMember))
                .ForMember(t => t.Email, m => m.MapFrom(o => o.Email))
                .ForMember(t => t.Birthday, m => m.MapFrom(o => o.Birthday))
                .ForMember(t => t.Created, m => m.Ignore())
                .ForMember(t => t.Name, m => m.MapFrom(t => t.Name))
                .ForMember(t => t.PasswordHash, m => m.Ignore())
                .ForMember(t => t.PasswordSalt, m => m.Ignore())
                .ForMember(t => t.Phone, m => m.MapFrom(t => t.Phone))
                .ForMember(t => t.Updated, m => m.Ignore())
                .ForMember(t => t.LinkHash, m => m.Ignore())
                .ForMember(t => t.IsActive, m => m.Ignore())
                .ForMember(t => t.RefreshToken, m => m.Ignore());
        }
    }
}
