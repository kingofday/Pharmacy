using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.Notifier.DataAccess.Ef.Migrations
{
    public partial class AddIsLockField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLock",
                schema: "Notifier",
                table: "Notification",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLock",
                schema: "Notifier",
                table: "Notification");
        }
    }
}
