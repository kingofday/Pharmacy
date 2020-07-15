using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.DataAccess.Ef.Migrations.AppDb
{
    public partial class EditPasswordLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                schema: "Base",
                table: "User",
                type: "char(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(25)",
                oldMaxLength: 25);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                schema: "Base",
                table: "User",
                type: "char(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(50)",
                oldMaxLength: 50);
        }
    }
}
