using bgTeam.DataAccess.Impl.Dapper;
using System;

namespace PartyMaker.Domain.Entities
{
    [Schema("public")]
    [TableName("user")]
    public class User : Entity
    {
        [ColumnName("username")]
        public string Name { get; set; }

        [ColumnName("email")]
        public string Email { get; set; }

        [ColumnName("image_id")]
        public Guid ImageId { get; set; }

        [ColumnName("phone")]
        public string Phone { get; set; }

        [ColumnName("userrole")]
        public EUserRole UserRole { get; set; }

        [ColumnName("passwordhash")]
        public byte[] PasswordHash { get; set; }

        [ColumnName("passwordsalt")]
        public byte[] PasswordSalt { get; set; }

        [ColumnName("refresh")]
        public Guid RefreshToken { get; set; }

        [ColumnName("linkhash")]
        public string LinkHash { get; set; }

        [ColumnName("isactive")]
        public bool IsActive { get; set; }

        [ColumnName("birthday")]
        public DateTime? Birthday { get; set; }
    }
}
