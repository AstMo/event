namespace PartyMaker.Schema.Schema
{
    using FluentMigrator;

    [Migration(1)]
    public class V01_UserTable : Migration
    {
        private const string _tableName = SchemaConstants.UserTableName;
        private const string _fileTableName = SchemaConstants.FileTableName;

        public override void Down()
        {
            Delete.Table(_tableName);
            Delete.Table(_fileTableName);
        }

        public override void Up()
        {
            Create.Table(_tableName)
                .WithColumn("id")
                    .AsGuid()
                    .PrimaryKey($"{_tableName}_pkey")
                .WithColumn("created")
                    .AsDateTime()
                    .Nullable()
                .WithColumn("updated")
                    .AsDateTime()
                    .Nullable()
                .WithColumn("isdeleted")
                    .AsBoolean()
                    .Nullable()
                .WithColumn("username")
                    .AsString(255)
                    .Nullable()
                .WithColumn("passwordhash")
                    .AsBinary()
                    .Nullable()
                .WithColumn("passwordsalt")
                    .AsBinary()
                    .Nullable()
                .WithColumn("userrole")
                    .AsString()
                    .Nullable();

            Create.Table(_fileTableName)
                .WithColumn("id")
                    .AsGuid()
                    .PrimaryKey($"{_fileTableName}_pkey")
                .WithColumn("created")
                    .AsDateTime()
                    .Nullable()
                .WithColumn("updated")
                    .AsDateTime()
                    .Nullable()
                .WithColumn("isdeleted")
                    .AsBoolean()
                    .Nullable()
                .WithColumn("filename")
                    .AsString()
                    .Nullable()
                .WithColumn("realpath")
                    .AsString()
                    .Nullable();
        }
    }
}
