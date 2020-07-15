using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.DataAccess.Ef.Migrations.AppDb
{
    public partial class AddTempOrderDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliverTrackingId",
                schema: "Order",
                table: "Order");

            migrationBuilder.CreateTable(
                name: "TempOrderDetail",
                schema: "Order",
                columns: table => new
                {
                    TempOrderDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasketId = table.Column<Guid>(nullable: false),
                    DrugId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<int>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempOrderDetail", x => x.TempOrderDetailId);
                    table.ForeignKey(
                        name: "FK_TempOrderDetail_Drug_DrugId",
                        column: x => x.DrugId,
                        principalSchema: "Store",
                        principalTable: "Drug",
                        principalColumn: "DrugId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketId",
                schema: "Order",
                table: "TempOrderDetail",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_TempOrderDetail_DrugId",
                schema: "Order",
                table: "TempOrderDetail",
                column: "DrugId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TempOrderDetail",
                schema: "Order");

            migrationBuilder.AddColumn<string>(
                name: "DeliverTrackingId",
                schema: "Order",
                table: "Order",
                type: "varchar(32)",
                maxLength: 32,
                nullable: true);
        }
    }
}
