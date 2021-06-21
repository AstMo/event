using bgTeam.DataAccess.Impl.Dapper;

namespace PartyMaker.Domain.Entities
{
    [TableName("localizations")]
    public class Localization : Entity
    {
        [ColumnName("name")]
        public virtual string Name { get; set; }
    }
}
