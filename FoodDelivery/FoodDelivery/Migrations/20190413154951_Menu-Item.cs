using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodDelivery.Migrations
{
    public partial class MenuItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_MenuItems_MenuItemId",
                table: "Baskets");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_Baskets_BasketId",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_BasketId",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_MenuItemId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "MenuItemId",
                table: "Baskets");

            migrationBuilder.RenameColumn(
                name: "BasketId",
                table: "MenuItems",
                newName: "Image");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MenuItems",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BasketItems",
                columns: table => new
                {
                    BasketId = table.Column<string>(nullable: false),
                    MenuItemId = table.Column<string>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItems", x => new { x.BasketId, x.MenuItemId });
                    table.ForeignKey(
                        name: "FK_BasketItems_Baskets_BasketId",
                        column: x => x.BasketId,
                        principalTable: "Baskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BasketItems_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_MenuItemId",
                table: "BasketItems",
                column: "MenuItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasketItems");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "MenuItems",
                newName: "BasketId");

            migrationBuilder.AlterColumn<string>(
                name: "BasketId",
                table: "MenuItems",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MenuItemId",
                table: "Baskets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_BasketId",
                table: "MenuItems",
                column: "BasketId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_Baskets_BasketId",
                table: "MenuItems",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
