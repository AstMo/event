using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Translator;
using PartyMaker.Common.Validation;
using PartyMaker.Domain.Entities;
using PartyMaker.Story.CommonStories;
using System;

namespace PartyMaker.Story.WebApplication.ExpenseItem
{
    public class WebAppCreateTaskStory : CreateEntityStory<TaskEvent, WebAppCreateExpenseItemStoryContext>, IStory<WebAppCreateExpenseItemStoryContext, Dto.WebApp.WebAppResponseDto>
    {
        private readonly ITranslatorFactory _translatorFactory;
        private readonly ICrudService _crudService;
        private readonly IConnectionFactory _connectionFactory;

        public WebAppCreateTaskStory(ITranslatorFactory translatorFactory,
            ICrudService crudService,
            IConnectionFactory connectionFactory,
            IWebAppValidatorFactory webAppValidatorFactory,
            IWebApproverFactory webApproverFactory,
            IAppLogger appLogger)
            : base(webApproverFactory, webAppValidatorFactory, translatorFactory, crudService, appLogger)
        {
            _translatorFactory = translatorFactory;
            _crudService = crudService;
            _connectionFactory = connectionFactory;
        }
        protected async override System.Threading.Tasks.Task AfterRequest(WebAppCreateExpenseItemStoryContext context, Guid id)
        {

        }
    }
}
