using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class removeFrequencyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsageFrequencys");

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Customers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Customers");

            migrationBuilder.CreateTable(
                name: "UsageFrequencys",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    EventClick = table.Column<int>(type: "int", nullable: true),
                    NewsClick = table.Column<int>(type: "int", nullable: true),
                    UseDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UseDuration = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsageFrequencys", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UsageFrequencys_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UsageFrequencys_CustomerID",
                table: "UsageFrequencys",
                column: "CustomerID");
        }
    }
}
