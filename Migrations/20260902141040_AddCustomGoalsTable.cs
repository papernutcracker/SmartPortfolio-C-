using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Dividend_Portfolio_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomGoalsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goals_Users_UserProfileId",
                table: "Goals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Goals",
                table: "Goals");

            migrationBuilder.RenameTable(
                name: "Goals",
                newName: "CustomGoal");

            migrationBuilder.RenameIndex(
                name: "IX_Goals_UserProfileId",
                table: "CustomGoal",
                newName: "IX_CustomGoal_UserProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomGoal",
                table: "CustomGoal",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomGoal_Users_UserProfileId",
                table: "CustomGoal",
                column: "UserProfileId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomGoal_Users_UserProfileId",
                table: "CustomGoal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomGoal",
                table: "CustomGoal");

            migrationBuilder.RenameTable(
                name: "CustomGoal",
                newName: "Goals");

            migrationBuilder.RenameIndex(
                name: "IX_CustomGoal_UserProfileId",
                table: "Goals",
                newName: "IX_Goals_UserProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Goals",
                table: "Goals",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_Users_UserProfileId",
                table: "Goals",
                column: "UserProfileId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
