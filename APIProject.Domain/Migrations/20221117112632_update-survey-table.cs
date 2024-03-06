using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class updatesurveytable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswers_SurveyQuestions_SurveyQuestionsID",
                table: "SurveyAnswers");

            migrationBuilder.DropIndex(
                name: "IX_SurveyAnswers_SurveyQuestionsID",
                table: "SurveyAnswers");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "SurveySheets");

            migrationBuilder.DropColumn(
                name: "SurveyQuestionsID",
                table: "SurveyAnswers");

            migrationBuilder.RenameColumn(
                name: "QuestionSurveyID",
                table: "SurveyAnswers",
                newName: "SurveyQuestionID");

            migrationBuilder.RenameColumn(
                name: "Answers",
                table: "SurveyAnswers",
                newName: "Answer");

            migrationBuilder.AddColumn<int>(
                name: "SurveyAnswerID",
                table: "SurveySheets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SurveyQuestionID",
                table: "SurveySheets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AgeType",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveySheets_SurveyQuestionID",
                table: "SurveySheets",
                column: "SurveyQuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAnswers_SurveyQuestionID",
                table: "SurveyAnswers",
                column: "SurveyQuestionID");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswers_SurveyQuestions_SurveyQuestionID",
                table: "SurveyAnswers",
                column: "SurveyQuestionID",
                principalTable: "SurveyQuestions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveySheets_SurveyQuestions_SurveyQuestionID",
                table: "SurveySheets",
                column: "SurveyQuestionID",
                principalTable: "SurveyQuestions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswers_SurveyQuestions_SurveyQuestionID",
                table: "SurveyAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveySheets_SurveyQuestions_SurveyQuestionID",
                table: "SurveySheets");

            migrationBuilder.DropIndex(
                name: "IX_SurveySheets_SurveyQuestionID",
                table: "SurveySheets");

            migrationBuilder.DropIndex(
                name: "IX_SurveyAnswers_SurveyQuestionID",
                table: "SurveyAnswers");

            migrationBuilder.DropColumn(
                name: "SurveyAnswerID",
                table: "SurveySheets");

            migrationBuilder.DropColumn(
                name: "SurveyQuestionID",
                table: "SurveySheets");

            migrationBuilder.DropColumn(
                name: "AgeType",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "SurveyQuestionID",
                table: "SurveyAnswers",
                newName: "QuestionSurveyID");

            migrationBuilder.RenameColumn(
                name: "Answer",
                table: "SurveyAnswers",
                newName: "Answers");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "SurveySheets",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "SurveyQuestionsID",
                table: "SurveyAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAnswers_SurveyQuestionsID",
                table: "SurveyAnswers",
                column: "SurveyQuestionsID");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswers_SurveyQuestions_SurveyQuestionsID",
                table: "SurveyAnswers",
                column: "SurveyQuestionsID",
                principalTable: "SurveyQuestions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
