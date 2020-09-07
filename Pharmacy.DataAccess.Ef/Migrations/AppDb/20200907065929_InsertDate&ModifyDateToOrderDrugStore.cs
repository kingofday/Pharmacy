using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.DataAccess.Ef.Migrations.AppDb
{
    public partial class InsertDateModifyDateToOrderDrugStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDateMi",
                schema: "Order",
                table: "OrderDrugStore",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InsertDateSh",
                schema: "Order",
                table: "OrderDrugStore",
                type: "char(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDateMi",
                schema: "Order",
                table: "OrderDrugStore",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifyDateSh",
                schema: "Order",
                table: "OrderDrugStore",
                type: "char(10)",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsertDateMi",
                schema: "Order",
                table: "OrderDrugStore");

            migrationBuilder.DropColumn(
                name: "InsertDateSh",
                schema: "Order",
                table: "OrderDrugStore");

            migrationBuilder.DropColumn(
                name: "ModifyDateMi",
                schema: "Order",
                table: "OrderDrugStore");

            migrationBuilder.DropColumn(
                name: "ModifyDateSh",
                schema: "Order",
                table: "OrderDrugStore");
        }
    }
}
