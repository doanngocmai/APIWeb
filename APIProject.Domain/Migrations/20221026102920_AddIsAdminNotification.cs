using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class AddIsAdminNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsAdmin",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NewsID",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NewsID",
                table: "Notifications",
                column: "NewsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_News_NewsID",
                table: "Notifications",
                column: "NewsID",
                principalTable: "News",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_News_NewsID",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_NewsID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "NewsID",
                table: "Notifications");
        }
    }
}
