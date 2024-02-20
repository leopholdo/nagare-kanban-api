using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace negare_kanban_api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBoardAddIsClosed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Boards",
                newName: "IsClosed");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosingDate",
                table: "Boards",
                type: "timestamp without time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingDate",
                table: "Boards");

            migrationBuilder.RenameColumn(
                name: "IsClosed",
                table: "Boards",
                newName: "IsDeleted");
        }
    }
}
