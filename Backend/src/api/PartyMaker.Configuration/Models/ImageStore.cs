using PartyMaker.Configuration.Interfaces;

namespace PartyMaker.Configuration.Models
{
    public class ImageStoreSettings : IImageStoreSetting
    {
        public string Url { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Path { get; set; }
    }
}
