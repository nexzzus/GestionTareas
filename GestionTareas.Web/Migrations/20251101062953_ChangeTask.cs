using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionTareas.Web.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "Tasks",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Tasks",
                newName: "status");
        }
    }
}
