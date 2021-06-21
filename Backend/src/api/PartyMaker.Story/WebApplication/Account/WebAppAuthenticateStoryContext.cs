using PartyMaker.Common.Request;

namespace PartyMaker.Story.WebApplication
{
    public class WebAppAuthenticateStoryContext : IRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
