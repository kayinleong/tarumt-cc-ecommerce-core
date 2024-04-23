using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tarumt.CC.Ecommerce.Infrastructure.Migrations.MySql
{
    /// <inheritdoc />
    public partial class MySql_14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductCarts_ProductCartId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductCartId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductCartId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ProductCartItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Count = table.Column<int>(type: "int", nullable: false),
                    ProductCartId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Remarks = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCartItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCartItem_ProductCarts_ProductCartId",
                        column: x => x.ProductCartId,
                        principalTable: "ProductCarts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductCartItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCartItem_ProductCartId",
                table: "ProductCartItem",
                column: "ProductCartId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCartItem_ProductId",
                table: "ProductCartItem",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCartItem");

            migrationBuilder.AddColumn<string>(
                name: "ProductCartId",
                table: "Products",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCartId",
                table: "Products",
                column: "ProductCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductCarts_ProductCartId",
                table: "Products",
                column: "ProductCartId",
                principalTable: "ProductCarts",
                principalColumn: "Id");
        }
    }
}
