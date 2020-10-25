using Microsoft.EntityFrameworkCore.Migrations;

namespace tes3mp_verifier.Data.Migrations
{
    public partial class VerificationOneToOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Verifications_UserId",
                table: "Verifications");

            migrationBuilder.CreateIndex(
                name: "IX_Verifications_UserId",
                table: "Verifications",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Verifications_UserId",
                table: "Verifications");

            migrationBuilder.CreateIndex(
                name: "IX_Verifications_UserId",
                table: "Verifications",
                column: "UserId");
        }
    }
}
