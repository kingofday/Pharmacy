using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.Notifier.DataAccess.Ef.Migrations
{
    public partial class AddApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                schema: "Notifier",
                table: "Notification",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Application",
                schema: "Notifier",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Token = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Application", x => x.ApplicationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Application",
                schema: "Notifier");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                schema: "Notifier",
                table: "Notification");
        }
    }
}
