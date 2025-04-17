using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Outbox.Infra.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class messagestables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxMessages",
                schema: "Worker",
                table: "OutboxMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InboxMessages",
                schema: "Worker",
                table: "InboxMessages");

            migrationBuilder.EnsureSchema(
                name: "Messages");

            migrationBuilder.RenameTable(
                name: "OutboxMessages",
                schema: "Worker",
                newName: "Outbox",
                newSchema: "Messages");

            migrationBuilder.RenameTable(
                name: "InboxMessages",
                schema: "Worker",
                newName: "Inbox",
                newSchema: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "Error",
                schema: "Messages",
                table: "Outbox",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                schema: "Messages",
                table: "Outbox",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Outbox",
                schema: "Messages",
                table: "Outbox",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inbox",
                schema: "Messages",
                table: "Inbox",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Outbox",
                schema: "Messages",
                table: "Outbox");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inbox",
                schema: "Messages",
                table: "Inbox");

            migrationBuilder.DropColumn(
                name: "Error",
                schema: "Messages",
                table: "Outbox");

            migrationBuilder.DropColumn(
                name: "RetryCount",
                schema: "Messages",
                table: "Outbox");

            migrationBuilder.EnsureSchema(
                name: "Worker");

            migrationBuilder.RenameTable(
                name: "Outbox",
                schema: "Messages",
                newName: "OutboxMessages",
                newSchema: "Worker");

            migrationBuilder.RenameTable(
                name: "Inbox",
                schema: "Messages",
                newName: "InboxMessages",
                newSchema: "Worker");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxMessages",
                schema: "Worker",
                table: "OutboxMessages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InboxMessages",
                schema: "Worker",
                table: "InboxMessages",
                column: "Id");
        }
    }
}
