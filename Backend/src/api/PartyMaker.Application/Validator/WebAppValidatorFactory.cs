using PartyMaker.Application.Validator.Account;
using PartyMaker.Application.Validator.Event;
using PartyMaker.Application.Validator.Task;
using PartyMaker.Common.Validation;
using PartyMaker.Story.WebApplication;
using PartyMaker.Story.WebApplication.Account;
using PartyMaker.Story.WebApplication.Event;
using PartyMaker.Story.WebApplication.Task;
using System;

namespace PartyMaker.Application.Validator
{
    public class WebAppValidatorFactory : ValidationFactory, IWebAppValidatorFactory
    {
        public WebAppValidatorFactory(IServiceProvider kernel)
            : base(kernel)
        {
        }

        protected override void RegisterValidators()
        {
            RegisterValidator<LoginAccountValidator, WebAppAuthenticateStoryContext>();
            RegisterValidator<RegistrationAccountValidator, WebAppRegistrationStoryContext>();
            RegisterValidator<GetUserValidator, WebAppGetAuthUserStoryContext>();
            RegisterValidator<RefreshTokenValidator, WebAppRefreshTokenRequestContext>();
            RegisterValidator<UpdateUserValidator, WebAppUpdateUserStoryContext>();
            RegisterValidator<CreateEventValidator, WebAppCreateEventStoryContext>();
            RegisterValidator<DeleteEventValidator, WebAppDeleteEventStoryContext>();
            RegisterValidator<UpdateEventValidator, WebAppUpdateEventStoryContext>();

            RegisterValidator<CreateTaskValidator, WebAppCreateTaskStoryContext>();
            RegisterValidator<DeleteTaskValidator, WebAppDeleteTaskStoryContext>();
            RegisterValidator<UpdateTaskValidator, WebAppUpdateTaskStoryContext>();

            RegisterValidator<ApproveEmailValidator, ApproveUserEmailStoryContext>();
            RegisterValidator<InviteUserValidator, WebAppInvitePersonByEmailStoryContext>();
            RegisterValidator<RegistrationByInviteValidator, WebAppRegistrationByInviteStoryContext>();
            RegisterValidator<RestoreUserValidator, WebAppRestorePasswordStoryContext>();
            RegisterValidator<ResendApprovingUserEmailValidator, ResendApprovingUserEmailStoryContext>();
        }
    }
}
