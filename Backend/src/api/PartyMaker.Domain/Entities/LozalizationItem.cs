using bgTeam.DataAccess.Impl.Dapper;
using System;

namespace PartyMaker.Domain.Entities
{
    [TableName("localizationitems")]
    public class LocalizationItem : Entity
    {
        [ColumnName("key")]
        public virtual string Key { get; set; }

        [ColumnName("value")]
        public virtual string Value { get; set; }

        [ColumnName("localization_id")]
        public virtual Guid LocalizationId { get; set; }
    }
}
