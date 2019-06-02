using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodDelivery.DAL.Migrations
{
    public partial class RemoveMenuItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MenuItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MenuItems");
        }
    }
}
