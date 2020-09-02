using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.DataAccess.Ef.Migrations.AppDb
{
    public partial class AddPrescriptionItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFixed",
                schema: "Order",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TempBasketId",
                schema: "Order",
                table: "Order");

            migrationBuilder.RenameIndex(
                name: "IX_Order_PrescriptionId",
                schema: "Order",
                table: "Order",
                newName: "PrescriptionId");

            migrationBuilder.CreateTable(
                name: "PrescriptionItem",
                schema: "Order",
                columns: table => new
                {
                    PrescriptionItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionId = table.Column<int>(nullable: false),
                    DrugId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    DiscountPrice = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionItem", x => x.PrescriptionItemId);
                    table.ForeignKey(
                        name: "FK_PrescriptionItem_Drug_DrugId",
                        column: x => x.DrugId,
                        principalSchema: "Drug",
                        principalTable: "Drug",
                        principalColumn: "DrugId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrescriptionItem_Prescription_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalSchema: "Order",
                        principalTable: "Prescription",
                        principalColumn: "PrescriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionItem_DrugId",
                schema: "Order",
                table: "PrescriptionItem",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionItem_PrescriptionId",
                schema: "Order",
                table: "PrescriptionItem",
                column: "PrescriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrescriptionItem",
                schema: "Order");

            migrationBuilder.RenameIndex(
                name: "PrescriptionId",
                schema: "Order",
                table: "Order",
                newName: "IX_Order_PrescriptionId");

            migrationBuilder.AddColumn<bool>(
                name: "IsFixed",
                schema: "Order",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TempBasketId",
                schema: "Order",
                table: "Order",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
