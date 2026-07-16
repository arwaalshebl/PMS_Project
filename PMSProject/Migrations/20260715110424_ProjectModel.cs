using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSProject.Migrations
{
    /// <inheritdoc />
    public partial class ProjectModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentProjectID = table.Column<int>(type: "int", nullable: true),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Projects_ParentProjectID",
                        column: x => x.ParentProjectID,
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectModelTaskModel",
                columns: table => new
                {
                    TasksId = table.Column<int>(type: "int", nullable: false),
                    projectsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectModelTaskModel", x => new { x.TasksId, x.projectsId });
                    table.ForeignKey(
                        name: "FK_ProjectModelTaskModel_Projects_projectsId",
                        column: x => x.projectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectModelTaskModel_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectModelTaskModel_projectsId",
                table: "ProjectModelTaskModel",
                column: "projectsId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ParentProjectID",
                table: "Projects",
                column: "ParentProjectID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectModelTaskModel");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
