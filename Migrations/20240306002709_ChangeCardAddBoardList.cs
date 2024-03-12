using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace negare_kanban_api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCardAddBoardList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoardListId",
                table: "Cards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_BoardListId",
                table: "Cards",
                column: "BoardListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_BoardLists_BoardListId",
                table: "Cards",
                column: "BoardListId",
                principalTable: "BoardLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_BoardLists_BoardListId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_BoardListId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "BoardListId",
                table: "Cards");
        }
    }
}
