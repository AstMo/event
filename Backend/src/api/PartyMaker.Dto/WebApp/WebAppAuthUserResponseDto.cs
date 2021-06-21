using PartyMaker.Domain.Entities;
using System;

namespace PartyMaker.Dto.WebApp
{
    public class WebAppAuthUserResponseDto : WebAppEntityDto
    {
        public string Username { get; set; }

        public EUserRole UserRole { get; set; }

        public string Email { get; set; }

        public Guid ImageId { get; set; }

        public string Phone { get; set; }

        public DateTime? Birthday { get; set; }
    }
}
