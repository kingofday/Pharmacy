using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.DataAccess.Ef.Migrations.AppDb
{
    public partial class RemoveAddressIdFromDrugStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressId",
                schema: "Drug",
                table: "DrugStore");

            migrationBuilder.RenameTable(
                name: "DrugStoreAddress",
                schema: "Base",
                newName: "DrugStoreAddress",
                newSchema: "Drug");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "DrugStoreAddress",
                schema: "Drug",
                newName: "DrugStoreAddress",
                newSchema: "Base");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                schema: "Drug",
                table: "DrugStore",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
