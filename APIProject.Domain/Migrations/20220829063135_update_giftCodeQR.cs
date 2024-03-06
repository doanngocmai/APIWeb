using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class update_giftCodeQR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerID",
                table: "GiftCodeQRs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GiftCodeQRs_CustomerID",
                table: "GiftCodeQRs",
                column: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCodeQRs_Customers_CustomerID",
                table: "GiftCodeQRs",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftCodeQRs_Customers_CustomerID",
                table: "GiftCodeQRs");

            migrationBuilder.DropIndex(
                name: "IX_GiftCodeQRs_CustomerID",
                table: "GiftCodeQRs");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "GiftCodeQRs");
        }
    }
}
