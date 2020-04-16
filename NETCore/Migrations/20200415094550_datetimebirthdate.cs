using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCore.Migrations
{
    public partial class datetimebirthdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "TB_M_Employee",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "BirthDate",
                table: "TB_M_Employee",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
