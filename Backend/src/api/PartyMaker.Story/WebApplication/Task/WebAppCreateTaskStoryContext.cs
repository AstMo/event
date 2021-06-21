using PartyMaker.Domain.Entities;
using PartyMaker.Dto.WebApp.ExpensesItem;
using PartyMaker.Story.CommonStories;
using System;
using System.Collections.Generic;

namespace PartyMaker.Story.WebApplication.Task
{
    public class WebAppCreateTaskStoryContext : CreateEntityStoryContext
    {
        public Guid EventId { get; set; }

        public Guid AssignedId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ETaskState State { get; set; }
    }
}
