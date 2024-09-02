using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class refactPlanes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "ActividadPlan",
                columns: table => new
                {
                    ActividadesId = table.Column<int>(type: "int", nullable: false),
                    PlanesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActividadPlan", x => new { x.ActividadesId, x.PlanesId });
                    table.ForeignKey(
                        name: "FK_ActividadPlan_Actividad_ActividadesId",
                        column: x => x.ActividadesId,
                        principalTable: "Actividad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActividadPlan_Planes_PlanesId",
                        column: x => x.PlanesId,
                        principalTable: "Planes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ActividadPlan_PlanesId",
                table: "ActividadPlan",
                column: "PlanesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActividadPlan");

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "Actividad",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Actividad_PlanId",
                table: "Actividad",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actividad_Planes_PlanId",
                table: "Actividad",
                column: "PlanId",
                principalTable: "Planes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
