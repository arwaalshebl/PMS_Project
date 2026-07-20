using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSProject.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
