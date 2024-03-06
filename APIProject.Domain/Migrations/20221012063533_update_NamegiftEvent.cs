using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class update_NamegiftEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuantitySold",
                table: "GiftEvent",
                newName: "QuantityExchanged");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuantityExchanged",
                table: "GiftEvent",
                newName: "QuantitySold");
        }
    }
}
