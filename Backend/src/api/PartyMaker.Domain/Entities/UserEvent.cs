using bgTeam.DataAccess.Impl.Dapper;
using System;

namespace PartyMaker.Domain.Entities
{
    [Schema("public")]
    [TableName("userevent")]
    public class UserEvent : Entity
    {
        [ColumnName("userid")]
        public Guid UserId { get; set; }
    
        [ColumnName("eventid")]
        public Guid EventId { get; set; }

        [ColumnName("role")]
        public EUserEventRole Role { get; set; }
    }
}
