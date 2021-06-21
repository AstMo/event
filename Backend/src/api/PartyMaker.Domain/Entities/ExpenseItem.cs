using bgTeam.DataAccess.Impl.Dapper;
using System;

namespace PartyMaker.Domain.Entities
{
    [Schema("public")]
    [TableName("expenseitem")]
    public class ExpenseItem : Entity
    {
        [ColumnName("name")]
        public string Name { get; set; }

        [ColumnName("eventid")]
        public Guid EventId { get; set; }

        [ColumnName("taskid")]
        public Guid TaskId { get; set; }

        [ColumnName("assignedid")]
        public Guid AssignedId { get; set; }

        [ColumnName("price")]
        public decimal Price { get; set; }

        [ColumnName("description")]
        public string Description { get; set; }
    }
}
