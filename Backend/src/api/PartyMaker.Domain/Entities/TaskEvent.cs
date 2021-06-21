using bgTeam.DataAccess.Impl.Dapper;
using System;

namespace PartyMaker.Domain.Entities
{
    [Schema("public")]
    [TableName("taskevent")]
    public class TaskEvent : Entity
    {
        [ColumnName("eventid")]
        public Guid EventId { get; set; }
    
        [ColumnName("assignedid")]
        public Guid AssignedId { get; set; }

        [ColumnName("name")]
        public string Name { get; set; }

        [ColumnName("description")]
        public string Description { get; set; }

        [ColumnName("state")]
        public ETaskState State { get; set; }
    }
}
