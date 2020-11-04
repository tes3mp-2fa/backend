using Microsoft.EntityFrameworkCore.Migrations;

namespace tes3mp_verifier.Data.Migrations
{
    public partial class VerificationPasswordRenameToCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Verifications");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Verifications",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Verifications");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Verifications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
