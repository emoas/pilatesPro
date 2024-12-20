﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class cuposPendientesfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "CupoPendiente",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "CupoPendiente");
        }
    }
}
