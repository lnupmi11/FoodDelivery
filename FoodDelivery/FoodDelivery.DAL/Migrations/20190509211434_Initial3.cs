using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodDelivery.DAL.Migrations
{
    public partial class Initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EstimatedTime",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EstimatedTime",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(TimeSpan));
        }
    }
}
