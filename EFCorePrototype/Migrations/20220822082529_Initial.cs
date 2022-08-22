using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCorePrototype.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PerformanceEntities",
                columns: table => new
                {
                    KeyOne = table.Column<string>(type: "TEXT", nullable: false),
                    KeyTwo = table.Column<string>(type: "TEXT", nullable: false),
                    KeyThree = table.Column<string>(type: "TEXT", nullable: false),
                    KeyFour = table.Column<string>(type: "TEXT", nullable: false),
                    KeyFive = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceEntities", x => new { x.KeyOne, x.KeyTwo, x.KeyThree, x.KeyFour, x.KeyFive });
                });

            migrationBuilder.CreateTable(
                name: "SchoolEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassroomEntities",
                columns: table => new
                {
                    Floor = table.Column<int>(type: "INTEGER", nullable: false),
                    Room = table.Column<string>(type: "TEXT", nullable: false),
                    SchoolId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomEntities", x => new { x.Room, x.Floor });
                    table.ForeignKey(
                        name: "FK_ClassroomEntities_SchoolEntities_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomEntities_SchoolId",
                table: "ClassroomEntities",
                column: "SchoolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassroomEntities");

            migrationBuilder.DropTable(
                name: "PerformanceEntities");

            migrationBuilder.DropTable(
                name: "SchoolEntities");
        }
    }
}
