using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class AddNewsFestival : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GiftExchangedQuantity",
                table: "News",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GiftID",
                table: "News",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GiftQuantity",
                table: "News",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IsNotify",
                table: "News",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsPopup",
                table: "News",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "OrderMinValue",
                table: "News",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Point",
                table: "News",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_News_GiftID",
                table: "News",
                column: "GiftID");

            migrationBuilder.AddForeignKey(
                name: "FK_News_Gifts_GiftID",
                table: "News",
                column: "GiftID",
                principalTable: "Gifts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_Gifts_GiftID",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_GiftID",
                table: "News");

            migrationBuilder.DropColumn(
                name: "GiftExchangedQuantity",
                table: "News");

            migrationBuilder.DropColumn(
                name: "GiftID",
                table: "News");

            migrationBuilder.DropColumn(
                name: "GiftQuantity",
                table: "News");

            migrationBuilder.DropColumn(
                name: "IsNotify",
                table: "News");

            migrationBuilder.DropColumn(
                name: "IsPopup",
                table: "News");

            migrationBuilder.DropColumn(
                name: "OrderMinValue",
                table: "News");

            migrationBuilder.DropColumn(
                name: "Point",
                table: "News");
        }
    }
}
