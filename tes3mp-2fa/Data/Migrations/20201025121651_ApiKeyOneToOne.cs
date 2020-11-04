using Microsoft.EntityFrameworkCore.Migrations;

namespace tes3mp_verifier.Data.Migrations
{
    public partial class ApiKeyOneToOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApiKeys_GameServerId",
                table: "ApiKeys");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_GameServerId",
                table: "ApiKeys",
                column: "GameServerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApiKeys_GameServerId",
                table: "ApiKeys");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_GameServerId",
                table: "ApiKeys",
                column: "GameServerId");
        }
    }
}
