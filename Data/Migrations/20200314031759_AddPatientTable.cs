using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Covid19App.Data.Migrations
{
    public partial class AddPatientTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    Level = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    Lng = table.Column<double>(nullable: true),
                    Lat = table.Column<double>(nullable: true),
                    CreatedDt = table.Column<DateTime>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
