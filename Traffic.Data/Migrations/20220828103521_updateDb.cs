using Microsoft.EntityFrameworkCore.Migrations;

namespace Traffic.Data.Migrations
{
    public partial class updateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignHistorys_Users_UserId",
                table: "CampaignHistorys");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCampaign_Users_UserId",
                table: "UserCampaign");

            migrationBuilder.DropIndex(
                name: "IX_UserCampaign_UserId",
                table: "UserCampaign");

            migrationBuilder.DropIndex(
                name: "IX_CampaignHistorys_UserId",
                table: "CampaignHistorys");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserCampaign");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CampaignHistorys");

            migrationBuilder.AlterColumn<int>(
                name: "ImplementBy",
                table: "UserCampaign",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<int>(
                name: "ImplementBy",
                table: "CampaignHistorys",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ImplementBy",
                table: "UserCampaign",
                type: "int",
                maxLength: 250,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserCampaign",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ImplementBy",
                table: "CampaignHistorys",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CampaignHistorys",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCampaign_UserId",
                table: "UserCampaign",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignHistorys_UserId",
                table: "CampaignHistorys",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignHistorys_Users_UserId",
                table: "CampaignHistorys",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCampaign_Users_UserId",
                table: "UserCampaign",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
