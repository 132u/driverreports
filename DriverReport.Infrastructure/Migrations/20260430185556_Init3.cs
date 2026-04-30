using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriverReport.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Reports",
                newName: "UpdatedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Reports",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReportDate",
                table: "Reports",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReportDate",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Reports",
                newName: "Date");
        }
    }
}
