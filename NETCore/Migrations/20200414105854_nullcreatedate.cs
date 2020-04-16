using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCore.Migrations
{
    public partial class nullcreatedate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateDate",
                table: "TB_M_Department",
                nullable: true,
                oldClrType: typeof(DateTimeOffset));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateDate",
                table: "TB_M_Department",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);
        }
    }
}
