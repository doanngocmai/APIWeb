using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class _update_database_Survery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_SurveyAnswers_SurveyQuestions_SurveyQuestionID",
            //    table: "SurveyAnswers");

            migrationBuilder.DropIndex(
                name: "IX_SurveyAnswers_SurveyQuestionID",
                table: "SurveyAnswers");

            migrationBuilder.RenameColumn(
                name: "SurveyQuestionID",
                table: "SurveyAnswers",
                newName: "QuestionSurveyID");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswers_SurveyQuestions_SurveyQuestionsID",
                table: "SurveyAnswers");

            migrationBuilder.DropIndex(
                name: "IX_SurveyAnswers_SurveyQuestionsID",
                table: "SurveyAnswers");

            migrationBuilder.DropColumn(
                name: "SurveyQuestionsID",
                table: "SurveyAnswers");

            migrationBuilder.RenameColumn(
                name: "QuestionSurveyID",
                table: "SurveyAnswers",
                newName: "SurveyQuestionID");

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
        }
    }
}
