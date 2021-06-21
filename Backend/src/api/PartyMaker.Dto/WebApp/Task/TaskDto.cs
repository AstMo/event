using PartyMaker.Domain.Entities;
using PartyMaker.Dto.WebApp.ExpensesItem;
using System;
using System.Collections.Generic;

namespace PartyMaker.Dto.WebApp.Task
{
    public class TaskDto : WebAppEntityDto
    {
        public Guid EventId { get; set; }

        public Guid AssignedId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ETaskState State { get; set; }

        public IEnumerable<ExpenseItemDto> ExpenseItems { get; set; }
    }
}
