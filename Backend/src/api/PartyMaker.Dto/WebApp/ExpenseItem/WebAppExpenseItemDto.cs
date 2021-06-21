using System;

namespace PartyMaker.Dto.WebApp.ExpenseItem
{
    public class WebAppExpenseItemDto
    {
        public string Name { get; set; }

        public Guid EventId { get; set; }

        public Guid TaskId { get; set; }

        public Guid AssignedId { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }
    }
}
