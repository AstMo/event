using PartyMaker.Application.Approvers.Account;
using PartyMaker.Application.Approvers.Event;
using PartyMaker.Common.Approver;
using PartyMaker.Story.WebApplication;
using PartyMaker.Story.WebApplication.Account;
using PartyMaker.Story.WebApplication.Event;
using PartyMaker.Story.WebApplication.Task;
using System;

namespace PartyMaker.Application.Approvers
{
    public class WebAppApproverFactory : ApproverFactory, IWebApproverFactory
    {
        public WebAppApproverFactory(IServiceProvider kernel)
            : base(kernel)
        {
        }

        protected override void RegisterApprovers()
        {
            RegisterApprover<LoginApprover, WebAppAuthenticateStoryContext>();
            RegisterApprover<RegistrationApprover, WebAppRegistrationStoryContext>();
            RegisterApprover<RefreshTokenApprover, WebAppRefreshTokenRequestContext>();
            RegisterApprover<GetUserApprover, WebAppGetAuthUserStoryContext>();
            RegisterApprover<UpdateUserApprover, WebAppUpdateUserStoryContext>();
            RegisterApprover<CreateEventApprover, WebAppCreateEventStoryContext>();
            RegisterApprover<DeleteEventApprover, WebAppDeleteEventStoryContext>();
            RegisterApprover<UpdateEventApprove, WebAppUpdateEventStoryContext>();

            RegisterApprover<CreateTaskApprover, WebAppCreateTaskStoryContext>();
            RegisterApprover<DeleteTaskApprover, WebAppDeleteTaskStoryContext>();
            RegisterApprover<UpdateTaskApprove, WebAppUpdateTaskStoryContext>();

            RegisterApprover<ApproveEmailUserApprover, ApproveUserEmailStoryContext>();
            RegisterApprover<InviteUserApprover, WebAppInvitePersonByEmailStoryContext>();
            RegisterApprover<RegistrationByInviteApprover, WebAppRegistrationByInviteStoryContext>();
            RegisterApprover<RestoreUserApprover, WebAppRestorePasswordStoryContext>();
            RegisterApprover<ResendApprovingUserEmailApprover, ResendApprovingUserEmailStoryContext>();
        }
    }
}
