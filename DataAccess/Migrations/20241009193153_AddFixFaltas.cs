using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class AddFixFaltas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClaseId",
                table: "Faltas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Faltas_ClaseId",
                table: "Faltas",
                column: "ClaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faltas_Clases_ClaseId",
                table: "Faltas",
                column: "ClaseId",
                principalTable: "Clases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faltas_Clases_ClaseId",
                table: "Faltas");

            migrationBuilder.DropIndex(
                name: "IX_Faltas_ClaseId",
                table: "Faltas");

            migrationBuilder.DropColumn(
                name: "ClaseId",
                table: "Faltas");
        }
    }
}
