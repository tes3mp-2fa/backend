using Microsoft.EntityFrameworkCore.Migrations;

namespace tes3mp_verifier.Data.Migrations
{
    public partial class LoginKeyCompositePK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LoginKeys",
                table: "LoginKeys");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoginKeys",
                table: "LoginKeys",
                columns: new[] { "Key", "UserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LoginKeys",
                table: "LoginKeys");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoginKeys",
                table: "LoginKeys",
                column: "Key");
        }
    }
}
