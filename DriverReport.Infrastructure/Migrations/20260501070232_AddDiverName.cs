using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriverReport.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDiverName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "Reports",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "Reports");
        }
    }
}
