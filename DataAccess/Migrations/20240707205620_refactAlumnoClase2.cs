using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class refactAlumnoClase2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlumnoClase_Users_AlumnoId",
                table: "AlumnoClase");

            migrationBuilder.DropColumn(
                name: "IdAlumno",
                table: "AlumnoClase");

            migrationBuilder.AlterColumn<int>(
                name: "AlumnoId",
                table: "AlumnoClase",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AlumnoClase_Users_AlumnoId",
                table: "AlumnoClase",
                column: "AlumnoId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlumnoClase_Users_AlumnoId",
                table: "AlumnoClase");

            migrationBuilder.AlterColumn<int>(
                name: "AlumnoId",
                table: "AlumnoClase",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IdAlumno",
                table: "AlumnoClase",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_AlumnoClase_Users_AlumnoId",
                table: "AlumnoClase",
                column: "AlumnoId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
