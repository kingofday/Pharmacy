using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.DataAccess.Ef.Migrations.AppDb
{
    public partial class AddUniqueIdToOrderChangeOrderOrderStatusFieldName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderStatus",
                schema: "Order",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "Order",
                table: "Order",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "UniqueId",
                schema: "Order",
                table: "Order",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Order",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "UniqueId",
                schema: "Order",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "OrderStatus",
                schema: "Order",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
