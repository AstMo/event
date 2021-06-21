using PartyMaker.Domain.Entities;
using System;
using System.Collections.Generic;

namespace PartyMaker.Dto.WebApp.Event
{
    public class WebAppEventDto : WebAppEntityDto
    {

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Address { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public ETypeEvent TypeEvent { get; set; }

        public decimal TotalBudget { get; set; }

        public IEnumerable<WebAppUserEventDto> Participaties {get;set;}
    }
}
