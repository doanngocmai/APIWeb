using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class UpdateIndexNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "Stalls",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "News",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Categorys",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "Stalls");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "News");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Categorys");
        }
    }
}
