using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LotteryApi.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftsInOrder_Orders_OrderId",
                table: "GiftsInOrder");

            migrationBuilder.DropIndex(
                name: "IX_GiftsInOrder_OrderId",
                table: "GiftsInOrder");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "GiftsInOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "GiftsInOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GiftsInOrder_OrderId",
                table: "GiftsInOrder",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftsInOrder_Orders_OrderId",
                table: "GiftsInOrder",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
