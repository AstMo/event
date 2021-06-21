using PartyMaker.Configuration.Interfaces;

namespace PartyMaker.Configuration.Models
{
    public class WebAppConfigSettings : IWebAppConfigSettings
    {
        public string LinkFormat { get; set; }

        public string InviteLinkFormat { get; set; }
    }
}
