using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class updateTableEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GiftEventParticipants",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EventParticipantID = table.Column<int>(type: "int", nullable: false),
                    GiftID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftEventParticipants", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GiftEventParticipants_EventParticipants_EventParticipantID",
                        column: x => x.EventParticipantID,
                        principalTable: "EventParticipants",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GiftEventParticipants_Gifts_GiftID",
                        column: x => x.GiftID,
                        principalTable: "Gifts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GiftEventParticipants_EventParticipantID",
                table: "GiftEventParticipants",
                column: "EventParticipantID");

            migrationBuilder.CreateIndex(
                name: "IX_GiftEventParticipants_GiftID",
                table: "GiftEventParticipants",
                column: "GiftID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GiftEventParticipants");
        }
    }
}
