using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LotteryApi.Migrations
{
    /// <inheritdoc />
    public partial class update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "ShoppingCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "PackagesInOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "PackagesInCart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "GiftsInOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "GiftsInCart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Gifts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Donors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "PackagesInOrder");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "PackagesInCart");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "GiftsInOrder");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "GiftsInCart");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Donors");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Categories");
        }
    }
}
