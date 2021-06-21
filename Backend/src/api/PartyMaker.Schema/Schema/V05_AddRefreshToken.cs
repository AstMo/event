using FluentMigrator;

namespace PartyMaker.Schema.Schema
{
    [Migration(5)]
    public class V05_AddRefreshToken : Migration
    {
        private const string _userTablename = SchemaConstants.UserTableName;
        private const string _localizationTableName = SchemaConstants.Localization;
        private const string _localiationItemsTableName = SchemaConstants.LocalizationItems;
        public override void Down()
        {
            Delete
                 .Column("refresh")
                 .FromTable(_userTablename);

            Delete.Table(_localizationTableName);
            Delete.Table(_localiationItemsTableName);
        }

        public override void Up()
        {
            Alter
                .Table(_userTablename)
                .AddColumn("refresh")
                    .AsGuid()
                    .Nullable();

            Create.Table(_localizationTableName)
                .WithColumn("id")
                    .AsGuid()
                    .PrimaryKey($"{_localizationTableName}_pkey")
                .WithColumn("created")
                    .AsDateTime()
                    .Nullable()
                .WithColumn("updated")
                    .AsDateTime()
                    .Nullable()
                .WithColumn("isdeleted")
                    .AsBoolean()
                    .Nullable()
                .WithColumn("name")
                    .AsString()
                    .Nullable();

            Create.Table(_localiationItemsTableName)
                .WithColumn("id")
                    .AsGuid()
                    .PrimaryKey($"{_localiationItemsTableName}_pkey")
                .WithColumn("created")
                    .AsDateTime()
                    .Nullable()
                .WithColumn("updated")
                    .AsDateTime()
                    .Nullable()
                .WithColumn("isdeleted")
                    .AsBoolean()
                    .Nullable()
                .WithColumn("key")
                    .AsString()
                .WithColumn("value")
                    .AsString()
                .WithColumn("localization_id")
                    .AsGuid();
        }
    }
}
