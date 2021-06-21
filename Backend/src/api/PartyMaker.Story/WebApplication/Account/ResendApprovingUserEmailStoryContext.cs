using PartyMaker.Common.Request;

namespace PartyMaker.Story.WebApplication.Account
{
    public class ResendApprovingUserEmailStoryContext : IRequest
    {
        public string Email { get; set; }
    }
}
