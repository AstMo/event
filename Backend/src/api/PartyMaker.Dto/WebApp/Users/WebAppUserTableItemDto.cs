using System;

namespace PartyMaker.Dto.WebApp.Users
{
    public class WebAppUserTableItemDto : WebAppEntityDto
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }
    }
}
