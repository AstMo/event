using PartyMaker.Common.Request;

namespace PartyMaker.Story.WebApplication.Account
{
    public class ApproveUserEmailStoryContext : IRequest
    {
        public string Token { get; set; }
    }
}
