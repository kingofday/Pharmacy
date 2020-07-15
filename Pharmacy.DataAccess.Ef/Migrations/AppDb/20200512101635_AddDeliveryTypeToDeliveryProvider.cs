using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.DataAccess.Ef.Migrations.AppDb
{
    public partial class AddDeliveryTypeToDeliveryProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryType",
                schema: "Order",
                table: "Order");

            migrationBuilder.AddColumn<byte>(
                name: "DeliveryType",
                schema: "Order",
                table: "DeliveryProvider",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryType",
                schema: "Order",
                table: "DeliveryProvider");

            migrationBuilder.AddColumn<byte>(
                name: "DeliveryType",
                schema: "Order",
                table: "Order",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
