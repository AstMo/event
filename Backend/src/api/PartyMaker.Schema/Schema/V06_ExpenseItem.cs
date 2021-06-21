using FluentMigrator;

namespace PartyMaker.Schema.Schema
{
    [Migration(6)]
    public class V06_ExpenseItem : Migration
    {
        private const string _expenseItemTableItem = SchemaConstants.ExpenseItemTableName;

        public override void Down()
        {
            Delete.Table(_expenseItemTableItem);
        }

        public override void Up()
        {
            Create.Table(_expenseItemTableItem)
                 .WithColumn("id")
                    .AsGuid()
                    .PrimaryKey($"{_expenseItemTableItem}_pkey")
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
                .WithColumn("taskid")
                    .AsGuid()
                .WithColumn("name")
                    .AsString(100)
                .WithColumn("description")
                    .AsString(4000)
                .WithColumn("price")
                    .AsDecimal(12, 2);
        }
    }
}
