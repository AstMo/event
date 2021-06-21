using PartyMaker.Domain.Entities;
using System;

namespace PartyMaker.Dto.WebApp.Event
{
    public class WebAppUserEventDto
    {
        public Guid? UserId { get; set; }

        public EUserEventRole Role { get; set; }

        public string Name { get; set; }
    }
}
