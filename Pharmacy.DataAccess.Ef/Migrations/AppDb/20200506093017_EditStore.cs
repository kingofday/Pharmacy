using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.DataAccess.Ef.Migrations.AppDb
{
    public partial class EditStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification",
                schema: "Base");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "Store",
                table: "Store",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                schema: "Store",
                table: "Store",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                schema: "Store",
                table: "Store",
                type: "varchar(25)",
                maxLength: 25,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "Store",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                schema: "Store",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "Username",
                schema: "Store",
                table: "Store");

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "Base",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ExtraData = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsertDateMi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Receiver = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    SendDateMi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SendStatus = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId);
                });
        }
    }
}
