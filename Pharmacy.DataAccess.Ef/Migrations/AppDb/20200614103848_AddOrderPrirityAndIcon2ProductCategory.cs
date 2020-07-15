using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.DataAccess.Ef.Migrations.AppDb
{
    public partial class AddOrderPrirityAndIcon2DrugCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                schema: "Base",
                table: "DrugCategory",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OrderPriority",
                schema: "Base",
                table: "DrugCategory",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                schema: "Base",
                table: "DrugCategory");

            migrationBuilder.DropColumn(
                name: "OrderPriority",
                schema: "Base",
                table: "DrugCategory");
        }
    }
}
