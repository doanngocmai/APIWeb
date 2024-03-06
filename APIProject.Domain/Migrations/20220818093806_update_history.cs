using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class update_history : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GiftID",
                table: "PointHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResidualPoint",
                table: "PointHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PointHistories_GiftID",
                table: "PointHistories",
                column: "GiftID");

            migrationBuilder.AddForeignKey(
                name: "FK_PointHistories_Gifts_GiftID",
                table: "PointHistories",
                column: "GiftID",
                principalTable: "Gifts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointHistories_Gifts_GiftID",
                table: "PointHistories");

            migrationBuilder.DropIndex(
                name: "IX_PointHistories_GiftID",
                table: "PointHistories");

            migrationBuilder.DropColumn(
                name: "GiftID",
                table: "PointHistories");

            migrationBuilder.DropColumn(
                name: "ResidualPoint",
                table: "PointHistories");
        }
    }
}
