using bgTeam.DataAccess.Impl.Dapper;
using System;

namespace PartyMaker.Domain.Entities
{
    public class Entity
    {
        [ColumnName("id")]
        //[Identity]
        [PrymaryKey]
        public Guid Id { get; set; }

        [ColumnName("created")]
        public DateTime Created { get; set; }

        [ColumnName("updated")]
        public DateTime Updated { get; set; }

        [ColumnName("isdeleted")]
        public bool IsDeleted { get; set; }
    }
}
