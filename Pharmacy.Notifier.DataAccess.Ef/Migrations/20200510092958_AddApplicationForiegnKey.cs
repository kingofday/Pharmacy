using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.Notifier.DataAccess.Ef.Migrations
{
    public partial class AddApplicationForiegnKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Notification_ApplicationId",
                schema: "Notifier",
                table: "Notification",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Application_ApplicationId",
                schema: "Notifier",
                table: "Notification",
                column: "ApplicationId",
                principalSchema: "Notifier",
                principalTable: "Application",
                principalColumn: "ApplicationId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Application_ApplicationId",
                schema: "Notifier",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_ApplicationId",
                schema: "Notifier",
                table: "Notification");
        }
    }
}
