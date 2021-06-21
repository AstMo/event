using AutoMapper;
using PartyMaker.Application.Translator.Event;
using PartyMaker.Application.Translator.Localization;
using PartyMaker.Application.Translator.EventTask;
using PartyMaker.Application.Translator.ExpenseItem;
using PartyMaker.Application.Translator.User;
using PartyMaker.Application.Translator.UserEvent;
using PartyMaker.Common.Translator;
using PartyMaker.Dto.WebApp;
using PartyMaker.Dto.WebApp.Event;
using PartyMaker.Dto.WebApp.Localization;
using PartyMaker.Dto.WebApp.ExpensesItem;

using PartyMaker.Dto.WebApp.Users;
using PartyMaker.Story.WebApplication;
using PartyMaker.Story.WebApplication.Account;
using PartyMaker.Story.WebApplication.Event;
using PartyMaker.Story.WebApplication.ExpenseItem;
using PartyMaker.Story.WebApplication.Task;
using System;
using PartyMaker.Dto.WebApp.Task;

namespace PartyMaker.Application.Translator
{
    public class WebAppTranslatorsFactory : TranslatorFactory
    {
        public WebAppTranslatorsFactory(IServiceProvider kernel)
            : base(kernel)
        {
        }

        protected override void RegisterTranslators(IMapperConfigurationExpression configurationExpression)
        {
            RegisterTranslator<UserToWebAppAuthenticateResponseDtoTranslator, Domain.Entities.User, WebAppAuthenticateResponceDto>(configurationExpression);
            RegisterTranslator<WebAppRegistratioStoryContextToUserTranslator, WebAppRegistrationStoryContext, Domain.Entities.User>(configurationExpression);
            RegisterTranslator<WebAppUpdateUserStoryContextToUserTranslator, WebAppUpdateUserStoryContext, Domain.Entities.User>(configurationExpression);

            RegisterTranslator<EventToEventDtoTranslator, Domain.Entities.Event, WebAppEventDto>(configurationExpression);
            RegisterTranslator<WebAppCreateEventStoryContextToEventTranslator, WebAppCreateEventStoryContext, Domain.Entities.Event>(configurationExpression);
            RegisterTranslator<WebAppUpdateEventStoryContextToEventTranslator, WebAppUpdateEventStoryContext, Domain.Entities.Event>(configurationExpression);
            RegisterTranslator<UserEventToUserEventDtoTranslator, Domain.Entities.UserEvent, WebAppUserEventDto>(configurationExpression);
            RegisterTranslator<UserToUserTableItemDtoTranslator, Domain.Entities.User, WebAppUserTableItemDto>(configurationExpression);
            RegisterTranslator<LocalizationToLocalizationTableItemTranslator, Domain.Entities.Localization, WebAppLocaleTableItemDto>(configurationExpression);
            RegisterTranslator<LocalizationItemToLocalizationItemDtoTranslator, Domain.Entities.LocalizationItem, WebAppLocaleItemDto>(configurationExpression);
            RegisterTranslator<LocalizationToLocalizationItemTranslator, Domain.Entities.Localization, WebAppLocaleDto>(configurationExpression);

            RegisterTranslator<EventTaskToEventDtoTranslator, Domain.Entities.TaskEvent, TaskDto>(configurationExpression);
            RegisterTranslator<WebAppCreateTaskStoryContextToTaskDtoTranslator, WebAppCreateTaskStoryContext, Domain.Entities.TaskEvent>(configurationExpression);
            RegisterTranslator<WebAppUpdatedTaskStoryContextToTaskDtoTranslator, WebAppUpdateTaskStoryContext, Domain.Entities.TaskEvent>(configurationExpression);

            RegisterTranslator<ExpenceItemToExpenseItemDtoTranslator, Domain.Entities.ExpenseItem, ExpenseItemDto>(configurationExpression);
            RegisterTranslator<WebAppExpenseItemCreateStoryContextToExpenseItemTranslator, WebAppCreateExpenseItemStoryContext, Domain.Entities.ExpenseItem>(configurationExpression);
            RegisterTranslator<WebAppExpenseItemUpdateStoryContextToExpenseItemTranslator, WebAppUpdateExpenseItemStoryContext, Domain.Entities.ExpenseItem>(configurationExpression);

            RegisterTranslator<WebAppRegistrationByInviteStoryContextToUserTranslator, WebAppRegistrationByInviteStoryContext, Domain.Entities.User>(configurationExpression);
        }
    }
}
