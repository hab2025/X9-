using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabCo.X9.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountTypeAndValueToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "Bookings",
                newName: "DiscountValue");

            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                table: "Bookings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "DiscountValue",
                table: "Bookings",
                newName: "Discount");
        }
    }
}
