using Microsoft.EntityFrameworkCore.Migrations;

namespace Traffic.Data.Migrations
{
    public partial class addnewcolunm_remainingBudget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "UserCampaign",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RemainingBudget",
                table: "Campaigns",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TotalFinishedTask",
                table: "Campaigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalView",
                table: "Campaigns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserCampaign");

            migrationBuilder.DropColumn(
                name: "RemainingBudget",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "TotalFinishedTask",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "TotalView",
                table: "Campaigns");
        }
    }
}
