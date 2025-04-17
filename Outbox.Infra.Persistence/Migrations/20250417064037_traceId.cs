using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Outbox.Infra.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class traceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TraceId",
                schema: "Messages",
                table: "Outbox",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TraceId",
                schema: "Messages",
                table: "Inbox",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TraceId",
                schema: "Messages",
                table: "Outbox");

            migrationBuilder.DropColumn(
                name: "TraceId",
                schema: "Messages",
                table: "Inbox");
        }
    }
}
