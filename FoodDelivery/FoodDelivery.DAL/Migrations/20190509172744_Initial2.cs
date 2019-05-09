using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodDelivery.DAL.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_MenuItems_MenuItemId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_MenuItems_MenuItemId",
                table: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_MenuItemId",
                table: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_Categories_MenuItemId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "MenuItemId",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "MenuItemId",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "MenuItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiscountId",
                table: "MenuItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_CategoryId",
                table: "MenuItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_DiscountId",
                table: "MenuItems",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_Categories_CategoryId",
                table: "MenuItems",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_Discounts_DiscountId",
                table: "MenuItems",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_Categories_CategoryId",
                table: "MenuItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_Discounts_DiscountId",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_CategoryId",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_DiscountId",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "MenuItems");

            migrationBuilder.AddColumn<string>(
                name: "MenuItemId",
                table: "Discounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MenuItemId",
                table: "Categories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_MenuItemId",
                table: "Discounts",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_MenuItemId",
                table: "Categories",
                column: "MenuItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_MenuItems_MenuItemId",
                table: "Categories",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_MenuItems_MenuItemId",
                table: "Discounts",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
