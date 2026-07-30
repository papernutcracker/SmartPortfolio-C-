using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Dividend_Portfolio_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfileIdToStocks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserProfileId",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "Stocks");
        }
    }
}
