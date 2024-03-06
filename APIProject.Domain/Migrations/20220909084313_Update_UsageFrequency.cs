using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class Update_UsageFrequency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsageFrequencys",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EventClick = table.Column<int>(type: "int", nullable: true),
                    NewsClick = table.Column<int>(type: "int", nullable: true),
                    UseDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UseDuration = table.Column<int>(type: "int", nullable: true),
                    SumNewClick = table.Column<int>(type: "int", nullable: true),
                    SumEventClick = table.Column<int>(type: "int", nullable: true),
                    Averaged = table.Column<int>(type: "int", nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsageFrequencys");
        }
    }
}
