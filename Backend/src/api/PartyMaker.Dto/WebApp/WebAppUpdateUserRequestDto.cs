using System;

namespace PartyMaker.Dto.WebApp
{
    public class WebAppUpdateUserRequestDto
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public Guid ImageId { get; set; }

        public string Phone { get; set; }

        public string Birthday { get; set; }
    }
}
