using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class add_GiftEvenChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_Gifts_GiftID",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_GiftID",
                table: "News");

            migrationBuilder.AddColumn<int>(
                name: "NewsID",
                table: "Gifts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GiftEventChannel",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GiftID = table.Column<int>(type: "int", nullable: false),
                    NewsID = table.Column<int>(type: "int", nullable: false),
                    QuantitySold = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftEventChannel", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GiftEventChannel_Gifts_GiftID",
                        column: x => x.GiftID,
                        principalTable: "Gifts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GiftEventChannel_News_NewsID",
                        column: x => x.NewsID,
                        principalTable: "News",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_NewsID",
                table: "Gifts",
                column: "NewsID");

            migrationBuilder.CreateIndex(
                name: "IX_GiftEventChannel_GiftID",
                table: "GiftEventChannel",
                column: "GiftID");

            migrationBuilder.CreateIndex(
                name: "IX_GiftEventChannel_NewsID",
                table: "GiftEventChannel",
                column: "NewsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_News_NewsID",
                table: "Gifts",
                column: "NewsID",
                principalTable: "News",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_News_NewsID",
                table: "Gifts");

            migrationBuilder.DropTable(
                name: "GiftEventChannel");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_NewsID",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "NewsID",
                table: "Gifts");

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
    }
}
