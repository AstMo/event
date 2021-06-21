using PartyMaker.Common.Request;

namespace PartyMaker.Story.WebApplication.Account
{
    public class WebAppInvitePersonByEmailStoryContext : IRequest
    {
        public string Email { get; set; }
    }
}
