using FluentMigrator;

namespace PartyMaker.Schema.Schema
{
    [Migration(3)]
    public class V03_events : Migration
    {
        private const string _tableName = SchemaConstants.EventTableName;
        private const string _eventUserTableName = SchemaConstants.EventUserTableName;
        private const string _eventUserCostTableName = SchemaConstants.UserEventCostsTableName;
        private const string _taskEventUserTableName = SchemaConstants.TaskEventTableName;

        public override void Down()
        {
            Delete.Table(_tableName);
            Delete.Table(_eventUserTableName);
            Delete.Table(_taskEventUserTableName);
            Delete.Table(_eventUserCostTableName);
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
                .WithColumn("name")
                    .AsString(100)
                .WithColumn("date")
                    .AsDateTime()
                .WithColumn("address")
                    .AsString(255)
                .WithColumn("latitude")
                    .AsDecimal(15, 11)
                .WithColumn("longitude")
                    .AsDecimal(15, 11)
                .WithColumn("typeevent")
                    .AsInt32()
                .WithColumn("totalbudget")
                    .AsDecimal(15, 2);

            Create.Table(_eventUserTableName)
                .WithColumn("id")
                    .AsGuid()
                    .PrimaryKey($"{_eventUserTableName}_pkey")
                .WithColumn("created")
                    .AsDateTime()
                    .Nullable()
                .WithColumn("updated")
                    .AsDateTime()
                    .Nullable()
                .WithColumn("isdeleted")
                    .AsBoolean()
                    .Nullable()
                .WithColumn("userid")
                    .AsGuid()
                    .WithColumn("eventid")
                    .AsGuid()
                .WithColumn("role")
                    .AsInt32();

            Create.Table(_eventUserCostTableName)
                .WithColumn("id")
                    .AsGuid()
                    .PrimaryKey($"{_eventUserCostTableName}_pkey")
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
                    .AsString(100)
                .WithColumn("cost")
                    .AsDecimal(10, 2)
                    .WithColumn("eventuserid")
                    .AsGuid();

            Create.Table(_taskEventUserTableName)
                 .WithColumn("id")
                    .AsGuid()
                    .PrimaryKey($"{_taskEventUserTableName}_pkey")
                .WithColumn("created")
                    .AsDateTime()
                    .Nullable()
                .WithColumn("updated")
                    .AsDateTime()
                    .Nullable()
                .WithColumn("isdeleted")
                    .AsBoolean()
                    .Nullable()
                .WithColumn("eventid")
                    .AsGuid()
                .WithColumn("assignedid")
                    .AsGuid()
                .WithColumn("name")
                    .AsString(100)
                .WithColumn("description")
                    .AsString(4000)
                .WithColumn("state")
                    .AsInt32();
        }
    }
}
