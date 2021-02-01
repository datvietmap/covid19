using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Covid19App.Data.Migrations
{
    public partial class InitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InfectionDate",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Patients",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InfectionDate",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Patients");
        }
    }
}
