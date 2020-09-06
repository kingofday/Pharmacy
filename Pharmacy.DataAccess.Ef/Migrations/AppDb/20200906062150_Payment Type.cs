using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.DataAccess.Ef.Migrations.AppDb
{
    public partial class PaymentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Type",
                schema: "Payment",
                table: "Payment",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "Payment",
                table: "Payment");
        }
    }
}
