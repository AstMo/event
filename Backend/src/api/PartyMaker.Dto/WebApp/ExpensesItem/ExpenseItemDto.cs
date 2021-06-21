using System;

namespace PartyMaker.Dto.WebApp.ExpensesItem
{
    public class ExpenseItemDto : WebAppEntityDto
    {
        public string Name { get; set; }

        public Guid EventId { get; set; }

        public Guid TaskId { get; set; }

        public Guid AssignedId { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }
    }
}
