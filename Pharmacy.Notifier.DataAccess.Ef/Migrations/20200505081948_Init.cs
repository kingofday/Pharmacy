using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.Notifier.DataAccess.Ef.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Notifier");

            migrationBuilder.CreateTable(
                name: "EventMapper",
                schema: "Notifier",
                columns: table => new
                {
                    EventMapperId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<byte>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    NotifyStrategy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventMapper", x => x.EventMapperId);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "Notifier",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    TryCount = table.Column<byte>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    SendDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    SendStatus = table.Column<string>(maxLength: 25, nullable: true),
                    Receiver = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(maxLength: 50, nullable: true),
                    ExtraData = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventMapper",
                schema: "Notifier");

            migrationBuilder.DropTable(
                name: "Notification",
                schema: "Notifier");
        }
    }
}
