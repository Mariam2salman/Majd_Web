using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Majd_Web.Migrations
{
    /// <inheritdoc />
    public partial class s14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlumniExperiences",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Experience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExperienceYear = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descreption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlumniExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlumniExperiences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlumniExperiences_UserId",
                table: "AlumniExperiences",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlumniExperiences");
        }
    }
}
