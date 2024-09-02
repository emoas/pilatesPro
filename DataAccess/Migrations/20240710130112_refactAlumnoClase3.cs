using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class refactAlumnoClase3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlumnoClase_Clases_ClaseId",
                table: "AlumnoClase");

            migrationBuilder.AlterColumn<int>(
                name: "ClaseId",
                table: "AlumnoClase",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AlumnoClase_Clases_ClaseId",
                table: "AlumnoClase",
                column: "ClaseId",
                principalTable: "Clases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlumnoClase_Clases_ClaseId",
                table: "AlumnoClase");

            migrationBuilder.AlterColumn<int>(
                name: "ClaseId",
                table: "AlumnoClase",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AlumnoClase_Clases_ClaseId",
                table: "AlumnoClase",
                column: "ClaseId",
                principalTable: "Clases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
