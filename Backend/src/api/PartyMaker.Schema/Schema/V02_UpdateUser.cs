using FluentMigrator;

namespace PartyMaker.Schema.Schema
{
    [Migration(2)]
    public class V02_UpdateUser : Migration
    {
        private const string _tableName = SchemaConstants.UserTableName;
        private const string _fileTableName = SchemaConstants.FileTableName;

        public override void Down()
        {
            Delete
                .Column("email")
                .Column("phone")
                .Column("image_id")
                .FromTable(_tableName);
        }

        public override void Up()
        {
            Alter
                .Table(_tableName)
                .AddColumn("email")
                .AsString(100)
                .NotNullable()
                .AddColumn("phone")
                .AsString(15)
                .Nullable()
                .AddColumn("image_id")
                .AsGuid()
                .Nullable();

        }
    }
}
