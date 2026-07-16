using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSProject.Migrations
{
    /// <inheritdoc />
    public partial class updateProjectRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Projects_ParentProjectID",
                table: "Projects");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Projects_ParentProjectID",
                table: "Projects",
                column: "ParentProjectID",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Projects_ParentProjectID",
                table: "Projects");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Projects_ParentProjectID",
                table: "Projects",
                column: "ParentProjectID",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
