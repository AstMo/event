using PartyMaker.Domain.Entities;
using PartyMaker.Dto.WebApp.Event;
using PartyMaker.Story.CommonStories;
using System;
using System.Collections.Generic;

namespace PartyMaker.Story.WebApplication.Event
{
    public class WebAppCreateEventStoryContext : CreateEntityStoryContext
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Address { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public ETypeEvent TypeEvent { get; set; }

        public decimal TotalBudget { get; set; }

        public IEnumerable<WebAppUserEventDto> Participaties { get; set; }
    }
}
