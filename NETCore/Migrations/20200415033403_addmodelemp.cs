using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCore.Migrations
{
    public partial class addmodelemp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_M_Employee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: true),
                    UpdateDate = table.Column<DateTimeOffset>(nullable: true),
                    DeleteDate = table.Column<DateTimeOffset>(nullable: true),
                    Department_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_M_Employee_TB_M_Department_Department_Id",
                        column: x => x.Department_Id,
                        principalTable: "TB_M_Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Employee_Department_Id",
                table: "TB_M_Employee",
                column: "Department_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_M_Employee");
        }
    }
}
