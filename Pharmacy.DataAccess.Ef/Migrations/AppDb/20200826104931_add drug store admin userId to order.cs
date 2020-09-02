using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.DataAccess.Ef.Migrations.AppDb
{
    public partial class adddrugstoreadminuserIdtoorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Store_UserId",
                schema: "Order",
                table: "Order",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Store_UserId",
                schema: "Order",
                table: "Order");
        }
    }
}
