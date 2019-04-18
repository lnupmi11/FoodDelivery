using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodDelivery.Migrations
{
    public partial class Setmenuitemtobasketconnectionasmanytomany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MenuItemId",
                table: "Baskets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_MenuItemId",
                table: "Baskets",
                column: "MenuItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_MenuItems_MenuItemId",
                table: "Baskets",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_MenuItems_MenuItemId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_MenuItemId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "MenuItemId",
                table: "Baskets");
        }
    }
}
