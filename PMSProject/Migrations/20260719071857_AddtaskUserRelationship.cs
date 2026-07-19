using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSProject.Migrations
{
    /// <inheritdoc />
    public partial class AddtaskUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskModelId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TaskModelId",
                table: "AspNetUsers",
                column: "TaskModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Tasks_TaskModelId",
                table: "AspNetUsers",
                column: "TaskModelId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Tasks_TaskModelId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TaskModelId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TaskModelId",
                table: "AspNetUsers");
        }
    }
}
