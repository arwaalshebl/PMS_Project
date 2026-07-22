using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSProject.Migrations
{
    /// <inheritdoc />
    public partial class FixProjectUserRelation1toM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Projects_ProjectModelId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProjectModelId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProjectModelId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "AssignedUserId",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AssignedUserId",
                table: "Projects",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_AssignedUserId",
                table: "Projects",
                column: "AssignedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_AssignedUserId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_AssignedUserId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "ProjectModelId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProjectModelId",
                table: "AspNetUsers",
                column: "ProjectModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Projects_ProjectModelId",
                table: "AspNetUsers",
                column: "ProjectModelId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
