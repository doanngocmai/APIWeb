using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class update_SurveryQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SurveyQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnswerQuestionID",
                table: "SurveyAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAnswers_AnswerQuestionID",
                table: "SurveyAnswers",
                column: "AnswerQuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerQuestions_SurveyQuestionID",
                table: "AnswerQuestions",
                column: "SurveyQuestionID");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerQuestions_SurveyQuestions_SurveyQuestionID",
                table: "AnswerQuestions",
                column: "SurveyQuestionID",
                principalTable: "SurveyQuestions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswers_AnswerQuestions_AnswerQuestionID",
                table: "SurveyAnswers",
                column: "AnswerQuestionID",
                principalTable: "AnswerQuestions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerQuestions_SurveyQuestions_SurveyQuestionID",
                table: "AnswerQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswers_AnswerQuestions_AnswerQuestionID",
                table: "SurveyAnswers");

            migrationBuilder.DropIndex(
                name: "IX_SurveyAnswers_AnswerQuestionID",
                table: "SurveyAnswers");

            migrationBuilder.DropIndex(
                name: "IX_AnswerQuestions_SurveyQuestionID",
                table: "AnswerQuestions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SurveyQuestions");

            migrationBuilder.DropColumn(
                name: "AnswerQuestionID",
                table: "SurveyAnswers");
        }
    }
}
