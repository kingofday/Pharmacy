using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.Notifier.DataAccess.Ef.Migrations
{
    public partial class AddApplicationIdToEventMapper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                schema: "Notifier",
                table: "EventMapper",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EventMapper_ApplicationId",
                schema: "Notifier",
                table: "EventMapper",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventMapper_Application_ApplicationId",
                schema: "Notifier",
                table: "EventMapper",
                column: "ApplicationId",
                principalSchema: "Notifier",
                principalTable: "Application",
                principalColumn: "ApplicationId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventMapper_Application_ApplicationId",
                schema: "Notifier",
                table: "EventMapper");

            migrationBuilder.DropIndex(
                name: "IX_EventMapper_ApplicationId",
                schema: "Notifier",
                table: "EventMapper");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                schema: "Notifier",
                table: "EventMapper");
        }
    }
}
