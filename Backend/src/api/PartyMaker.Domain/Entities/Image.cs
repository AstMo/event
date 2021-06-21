using bgTeam.DataAccess.Impl.Dapper;

namespace PartyMaker.Domain.Entities
{
    [Schema("public")]
    [TableName("file")]
    public class Image : Entity
    {
        [ColumnName("filename")]
        public string Filename { get; set; }

        [ColumnName("realpath")]
        public string RealPath { get; set; }
    }
}
