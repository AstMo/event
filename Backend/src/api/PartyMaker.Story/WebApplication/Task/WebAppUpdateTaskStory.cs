using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Translator;
using PartyMaker.Common.Validation;
using PartyMaker.Domain.Entities;
using PartyMaker.Story.CommonStories;
using System;
using System.Linq;

namespace PartyMaker.Story.WebApplication.Task
{
    public class WebAppUpdateTaskStory : UpdateEntityStory<Domain.Entities.TaskEvent, WebAppUpdateTaskStoryContext>, IStory<WebAppUpdateTaskStoryContext, PartyMaker.Dto.WebApp.WebAppResponseDto>
    {
        private readonly ICrudService _crudService;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IRepository _repository;

        public WebAppUpdateTaskStory(ITranslatorFactory translatorFactory,
            ICrudService crudService,
            IConnectionFactory connectionFactory,
            IRepository repository,
            IWebAppValidatorFactory webAppValidatorFactory,
            IWebApproverFactory webApproverFactory,
            IAppLogger appLogger)
            : base(translatorFactory, repository, crudService,connectionFactory, webAppValidatorFactory, webApproverFactory,  appLogger)
        {
            _crudService = crudService;
            _connectionFactory = connectionFactory;
            _repository = repository;
        }
        protected async override System.Threading.Tasks.Task AfterRequest(WebAppUpdateTaskStoryContext context)
        {

        }
    }
}
