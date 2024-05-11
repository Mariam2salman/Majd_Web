using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Majd_Web.Migrations
{
    /// <inheritdoc />
    public partial class s15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Experience",
                table: "AlumniExperiences",
                newName: "ExperienceName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExperienceName",
                table: "AlumniExperiences",
                newName: "Experience");
        }
    }
}
