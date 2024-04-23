using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tarumt.CC.Ecommerce.Infrastructure.Migrations.MySql
{
    /// <inheritdoc />
    public partial class MySql_17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCartItem_Products_ProductId",
                table: "UserCartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCartItem_UserCarts_UserCartId",
                table: "UserCartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCartItem_UserOrders_UserOrderId",
                table: "UserCartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCartItem",
                table: "UserCartItem");

            migrationBuilder.RenameTable(
                name: "UserCartItem",
                newName: "UserCartItems");

            migrationBuilder.RenameIndex(
                name: "IX_UserCartItem_UserOrderId",
                table: "UserCartItems",
                newName: "IX_UserCartItems_UserOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCartItem_UserCartId",
                table: "UserCartItems",
                newName: "IX_UserCartItems_UserCartId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCartItem_ProductId",
                table: "UserCartItems",
                newName: "IX_UserCartItems_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCartItems",
                table: "UserCartItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCartItems_Products_ProductId",
                table: "UserCartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCartItems_UserCarts_UserCartId",
                table: "UserCartItems",
                column: "UserCartId",
                principalTable: "UserCarts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCartItems_UserOrders_UserOrderId",
                table: "UserCartItems",
                column: "UserOrderId",
                principalTable: "UserOrders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCartItems_Products_ProductId",
                table: "UserCartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCartItems_UserCarts_UserCartId",
                table: "UserCartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCartItems_UserOrders_UserOrderId",
                table: "UserCartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCartItems",
                table: "UserCartItems");

            migrationBuilder.RenameTable(
                name: "UserCartItems",
                newName: "UserCartItem");

            migrationBuilder.RenameIndex(
                name: "IX_UserCartItems_UserOrderId",
                table: "UserCartItem",
                newName: "IX_UserCartItem_UserOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCartItems_UserCartId",
                table: "UserCartItem",
                newName: "IX_UserCartItem_UserCartId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCartItems_ProductId",
                table: "UserCartItem",
                newName: "IX_UserCartItem_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCartItem",
                table: "UserCartItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCartItem_Products_ProductId",
                table: "UserCartItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCartItem_UserCarts_UserCartId",
                table: "UserCartItem",
                column: "UserCartId",
                principalTable: "UserCarts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCartItem_UserOrders_UserOrderId",
                table: "UserCartItem",
                column: "UserOrderId",
                principalTable: "UserOrders",
                principalColumn: "Id");
        }
    }
}
