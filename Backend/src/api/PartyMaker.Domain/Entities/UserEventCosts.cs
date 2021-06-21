using bgTeam.DataAccess.Impl.Dapper;
using System;

namespace PartyMaker.Domain.Entities
{
    [Schema("public")]
    [TableName("usereventcosts")]
    public class UserEventCosts : Entity
    {
        [ColumnName("name")]
        public string Name { get; set; }

        [ColumnName("cost")]
        public decimal Cost { get; set; }

        [ColumnName("eventuserid")]
        public Guid EventUserId { get; set; }
    }
}
