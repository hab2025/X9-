using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabCo.X9.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsConfirmed",
                table: "Bookings",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "EventDate",
                table: "Bookings",
                newName: "StartTime");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "Bookings",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<DateTime>(
                name: "EventDay",
                table: "Bookings",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "EventDay",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Bookings",
                newName: "IsConfirmed");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Bookings",
                newName: "EventDate");
        }
    }
}
