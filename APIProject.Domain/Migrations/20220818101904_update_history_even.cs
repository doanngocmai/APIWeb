using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class update_history_even : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointHistories_EventParticipants_EventParticipantID",
                table: "PointHistories");

            migrationBuilder.AlterColumn<int>(
                name: "EventParticipantID",
                table: "PointHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PointHistories_EventParticipants_EventParticipantID",
                table: "PointHistories",
                column: "EventParticipantID",
                principalTable: "EventParticipants",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointHistories_EventParticipants_EventParticipantID",
                table: "PointHistories");

            migrationBuilder.AlterColumn<int>(
                name: "EventParticipantID",
                table: "PointHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PointHistories_EventParticipants_EventParticipantID",
                table: "PointHistories",
                column: "EventParticipantID",
                principalTable: "EventParticipants",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
