using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriverReport.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedClientName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "Reports",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "Reports");
        }
    }
}
