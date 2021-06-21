using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PartyMaker.Schema.Schema
{
    [Migration(7)]
    public class V07_AddBirthdayToUser : Migration
    {
        private const string _tableName = SchemaConstants.UserTableName;

        public override void Down()
        {
            Delete
                .Column("birthday")
                .FromTable(_tableName);
        }

        public override void Up()
        {
            Alter
                .Table(_tableName)
                .AddColumn("birthday")
                .AsDate()
                .Nullable();

        }
    }
}
