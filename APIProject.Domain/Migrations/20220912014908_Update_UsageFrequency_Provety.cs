using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class Update_UsageFrequency_Provety : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Averaged",
                table: "UsageFrequencys");

            migrationBuilder.DropColumn(
                name: "SumEventClick",
                table: "UsageFrequencys");

            migrationBuilder.DropColumn(
                name: "SumNewClick",
                table: "UsageFrequencys");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Averaged",
                table: "UsageFrequencys",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SumEventClick",
                table: "UsageFrequencys",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SumNewClick",
                table: "UsageFrequencys",
                type: "int",
                nullable: true);
        }
    }
}
