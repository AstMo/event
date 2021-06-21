using PartyMaker.Configuration.Interfaces;

namespace PartyMaker.Configuration.Models
{
    public class AuthenticationSettings : IAuthenticationSettings
    {
        public string Secret { get; set; }
    }
}
