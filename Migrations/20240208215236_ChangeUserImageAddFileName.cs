using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace negare_kanban_api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserImageAddFileName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "UserImages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "UserImages",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "UserImages");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "UserImages");
        }
    }
}
