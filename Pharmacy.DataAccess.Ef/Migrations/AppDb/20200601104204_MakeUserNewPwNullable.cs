using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.DataAccess.Ef.Migrations.AppDb
{
    public partial class MakeUserNewPwNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Order_OrderId",
                schema: "Payment",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_PaymentGateway_PaymentGatewayId",
                schema: "Payment",
                table: "Payment");

            migrationBuilder.AlterColumn<string>(
                name: "NewPassword",
                schema: "Base",
                table: "User",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Order_OrderId",
                schema: "Payment",
                table: "Payment",
                column: "OrderId",
                principalSchema: "Order",
                principalTable: "Order",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_PaymentGateway_PaymentGatewayId",
                schema: "Payment",
                table: "Payment",
                column: "PaymentGatewayId",
                principalSchema: "Payment",
                principalTable: "PaymentGateway",
                principalColumn: "PaymentGatewayId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Order_OrderId",
                schema: "Payment",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_PaymentGateway_PaymentGatewayId",
                schema: "Payment",
                table: "Payment");

            migrationBuilder.AlterColumn<string>(
                name: "NewPassword",
                schema: "Base",
                table: "User",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Order_OrderId",
                schema: "Payment",
                table: "Payment",
                column: "OrderId",
                principalSchema: "Order",
                principalTable: "Order",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_PaymentGateway_PaymentGatewayId",
                schema: "Payment",
                table: "Payment",
                column: "PaymentGatewayId",
                principalSchema: "Payment",
                principalTable: "PaymentGateway",
                principalColumn: "PaymentGatewayId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
