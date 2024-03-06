using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class update_newws : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiftExchangedQuantity",
                table: "News");

            migrationBuilder.DropColumn(
                name: "GiftID",
                table: "News");

            migrationBuilder.RenameColumn(
                name: "GiftQuantity",
                table: "News",
                newName: "GiftLimit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GiftLimit",
                table: "News",
                newName: "GiftQuantity");

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
        }
    }
}
