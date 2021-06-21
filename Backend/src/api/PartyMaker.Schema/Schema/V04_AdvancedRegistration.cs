using FluentMigrator;

namespace PartyMaker.Schema.Schema
{
    [Migration(4)]
    public class V04_AdvancedReistration : Migration
    {
        private const string _tableName = SchemaConstants.UserTableName;

        public override void Down()
        {
            Delete
                 .Column("linkhash")
                 .Column("isactive")
                 .FromTable(_tableName);
        }

        public override void Up()
        {
            Alter
                .Table(_tableName)
                .AddColumn("linkhash")
                    .AsString()
                    .Nullable()
                .AddColumn("isactive")
                    .AsBoolean()
                    .Nullable();
        }
    }
}
