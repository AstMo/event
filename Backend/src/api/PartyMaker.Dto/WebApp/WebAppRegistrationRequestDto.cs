using System;

namespace PartyMaker.Dto.WebApp
{
    public class WebAppRegistrationRequestDto
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public Guid ImageId { get; set; }

        public string Password { get; set; }

        public string PasswordRepeat { get; set; }

        public string Phone { get; set; }

        public DateTime Birthday { get; set; }
    }
}
