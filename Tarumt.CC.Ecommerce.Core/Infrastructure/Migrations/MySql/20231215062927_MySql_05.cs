using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tarumt.CC.Ecommerce.Infrastructure.Migrations.MySql
{
    /// <inheritdoc />
    public partial class MySql_05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganiserId",
                table: "Events",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganiserId",
                table: "Events",
                column: "OrganiserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Organisers_OrganiserId",
                table: "Events",
                column: "OrganiserId",
                principalTable: "Organisers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Organisers_OrganiserId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_OrganiserId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "OrganiserId",
                table: "Events");
        }
    }
}
