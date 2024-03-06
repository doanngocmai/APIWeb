using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class update_NewgiftEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftEvent_Gifts_GiftID",
                table: "GiftEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_GiftEvent_News_NewsID",
                table: "GiftEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GiftEvent",
                table: "GiftEvent");

            migrationBuilder.RenameTable(
                name: "GiftEvent",
                newName: "GiftEvents");

            migrationBuilder.RenameIndex(
                name: "IX_GiftEvent_NewsID",
                table: "GiftEvents",
                newName: "IX_GiftEvents_NewsID");

            migrationBuilder.RenameIndex(
                name: "IX_GiftEvent_GiftID",
                table: "GiftEvents",
                newName: "IX_GiftEvents_GiftID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GiftEvents",
                table: "GiftEvents",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftEvents_Gifts_GiftID",
                table: "GiftEvents",
                column: "GiftID",
                principalTable: "Gifts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GiftEvents_News_NewsID",
                table: "GiftEvents",
                column: "NewsID",
                principalTable: "News",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftEvents_Gifts_GiftID",
                table: "GiftEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_GiftEvents_News_NewsID",
                table: "GiftEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GiftEvents",
                table: "GiftEvents");

            migrationBuilder.RenameTable(
                name: "GiftEvents",
                newName: "GiftEvent");

            migrationBuilder.RenameIndex(
                name: "IX_GiftEvents_NewsID",
                table: "GiftEvent",
                newName: "IX_GiftEvent_NewsID");

            migrationBuilder.RenameIndex(
                name: "IX_GiftEvents_GiftID",
                table: "GiftEvent",
                newName: "IX_GiftEvent_GiftID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GiftEvent",
                table: "GiftEvent",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftEvent_Gifts_GiftID",
                table: "GiftEvent",
                column: "GiftID",
                principalTable: "Gifts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GiftEvent_News_NewsID",
                table: "GiftEvent",
                column: "NewsID",
                principalTable: "News",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
