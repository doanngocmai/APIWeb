using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class AddQRCodeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillPrice",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "IdentityCard",
                table: "Customers",
                newName: "IdentityNumber");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Bills",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "BillImage",
                table: "Bills",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "BillCode",
                table: "Bills",
                newName: "Code");

            migrationBuilder.CreateTable(
                name: "QRCodes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProvinceID = table.Column<int>(type: "int", nullable: false),
                    DistrictID = table.Column<int>(type: "int", nullable: false),
                    WardID = table.Column<int>(type: "int", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    IdentityNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Job = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalPrice = table.Column<long>(type: "bigint", nullable: false),
                    NewsID = table.Column<int>(type: "int", nullable: false),
                    EventChannelID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRCodes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QRCodes_EventChannels_EventChannelID",
                        column: x => x.EventChannelID,
                        principalTable: "EventChannels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QRCodes_News_NewsID",
                        column: x => x.NewsID,
                        principalTable: "News",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QRCodeBills",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<int>(type: "int", nullable: false),
                    QRCodeID = table.Column<int>(type: "int", nullable: false),
                    StallID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRCodeBills", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QRCodeBills_QRCodes_QRCodeID",
                        column: x => x.QRCodeID,
                        principalTable: "QRCodes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QRCodeBills_Stalls_StallID",
                        column: x => x.StallID,
                        principalTable: "Stalls",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_QRCodeBills_QRCodeID",
                table: "QRCodeBills",
                column: "QRCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_QRCodeBills_StallID",
                table: "QRCodeBills",
                column: "StallID");

            migrationBuilder.CreateIndex(
                name: "IX_QRCodes_EventChannelID",
                table: "QRCodes",
                column: "EventChannelID");

            migrationBuilder.CreateIndex(
                name: "IX_QRCodes_NewsID",
                table: "QRCodes",
                column: "NewsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QRCodeBills");

            migrationBuilder.DropTable(
                name: "QRCodes");

            migrationBuilder.RenameColumn(
                name: "IdentityNumber",
                table: "Customers",
                newName: "IdentityCard");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Bills",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Bills",
                newName: "BillImage");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Bills",
                newName: "BillCode");

            migrationBuilder.AddColumn<int>(
                name: "BillPrice",
                table: "Bills",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
