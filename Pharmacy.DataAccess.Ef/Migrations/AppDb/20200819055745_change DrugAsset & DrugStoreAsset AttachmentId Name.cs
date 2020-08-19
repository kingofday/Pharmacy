using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.DataAccess.Ef.Migrations.AppDb
{
    public partial class changeDrugAssetDrugStoreAssetAttachmentIdName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugAsset",
                schema: "Drug");

            migrationBuilder.DropTable(
                name: "DrugStoreAsset",
                schema: "Drug");

            migrationBuilder.CreateTable(
                name: "DrugAttachment",
                schema: "Drug",
                columns: table => new
                {
                    DrugAttachmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileType = table.Column<byte>(nullable: false),
                    AttachmentType = table.Column<byte>(nullable: false),
                    Size = table.Column<long>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Extention = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    OriginalName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    FileName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Url = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    PhysicalPath = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    DrugId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugAttachment", x => x.DrugAttachmentId);
                    table.ForeignKey(
                        name: "FK_DrugAttachment_Drug_DrugId",
                        column: x => x.DrugId,
                        principalSchema: "Drug",
                        principalTable: "Drug",
                        principalColumn: "DrugId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrugStoreAttachment",
                schema: "Drug",
                columns: table => new
                {
                    DrugStoreAttachmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileType = table.Column<byte>(nullable: false),
                    AttachmentType = table.Column<byte>(nullable: false),
                    Size = table.Column<long>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Extention = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    OriginalName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    FileName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Url = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    PhysicalPath = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    DrugStoreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugStoreAttachment", x => x.DrugStoreAttachmentId);
                    table.ForeignKey(
                        name: "FK_DrugStoreAttachment_DrugStore_DrugStoreId",
                        column: x => x.DrugStoreId,
                        principalSchema: "Drug",
                        principalTable: "DrugStore",
                        principalColumn: "DrugStoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrugAttachment_DrugId",
                schema: "Drug",
                table: "DrugAttachment",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugStoreAttachment_DrugStoreId",
                schema: "Drug",
                table: "DrugStoreAttachment",
                column: "DrugStoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugAttachment",
                schema: "Drug");

            migrationBuilder.DropTable(
                name: "DrugStoreAttachment",
                schema: "Drug");

            migrationBuilder.CreateTable(
                name: "DrugAsset",
                schema: "Drug",
                columns: table => new
                {
                    DrugAssetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentType = table.Column<byte>(type: "tinyint", nullable: false),
                    DrugId = table.Column<int>(type: "int", nullable: false),
                    Extention = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    FileName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    FileType = table.Column<byte>(type: "tinyint", nullable: false),
                    InsertDateMi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    OriginalName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    PhysicalPath = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    Url = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugAsset", x => x.DrugAssetId);
                    table.ForeignKey(
                        name: "FK_DrugAsset_Drug_DrugId",
                        column: x => x.DrugId,
                        principalSchema: "Drug",
                        principalTable: "Drug",
                        principalColumn: "DrugId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrugStoreAsset",
                schema: "Drug",
                columns: table => new
                {
                    DrugStoreAssetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentType = table.Column<byte>(type: "tinyint", nullable: false),
                    DrugStoreId = table.Column<int>(type: "int", nullable: false),
                    Extention = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    FileName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    FileType = table.Column<byte>(type: "tinyint", nullable: false),
                    InsertDateMi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    OriginalName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    PhysicalPath = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    Url = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugStoreAsset", x => x.DrugStoreAssetId);
                    table.ForeignKey(
                        name: "FK_DrugStoreAsset_DrugStore_DrugStoreId",
                        column: x => x.DrugStoreId,
                        principalSchema: "Drug",
                        principalTable: "DrugStore",
                        principalColumn: "DrugStoreId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrugAsset_DrugId",
                schema: "Drug",
                table: "DrugAsset",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugStoreAsset_DrugStoreId",
                schema: "Drug",
                table: "DrugStoreAsset",
                column: "DrugStoreId");
        }
    }
}
