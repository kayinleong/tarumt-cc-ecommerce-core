using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tarumt.CC.Ecommerce.Infrastructure.Migrations.MySql
{
    /// <inheritdoc />
    public partial class MySql_03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequiredStrongPasswordValidation",
                table: "UserServerSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiredStrongPasswordValidation",
                table: "UserServerSettings");
        }
    }
}
