using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.DataAccess.Ef.Migrations.AppDb
{
    public partial class AddNewPassword2User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRecoveredPassword",
                schema: "Base",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                schema: "Base",
                table: "User",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<bool>(
                name: "MustChangePassword",
                schema: "Base",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NewPassword",
                schema: "Base",
                table: "User",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MustChangePassword",
                schema: "Base",
                table: "User");

            migrationBuilder.DropColumn(
                name: "NewPassword",
                schema: "Base",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                schema: "Base",
                table: "User",
                type: "char(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecoveredPassword",
                schema: "Base",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
