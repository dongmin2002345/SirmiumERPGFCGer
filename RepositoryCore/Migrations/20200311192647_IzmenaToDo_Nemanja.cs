using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmenaToDo_Nemanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ToDoStatusId",
                table: "ToDos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToDos_ToDoStatusId",
                table: "ToDos",
                column: "ToDoStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_ToDoStatuses_ToDoStatusId",
                table: "ToDos",
                column: "ToDoStatusId",
                principalTable: "ToDoStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_ToDoStatuses_ToDoStatusId",
                table: "ToDos");

            migrationBuilder.DropIndex(
                name: "IX_ToDos_ToDoStatusId",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "ToDoStatusId",
                table: "ToDos");
        }
    }
}
