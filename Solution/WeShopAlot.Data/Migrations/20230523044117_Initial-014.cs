using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShopAlot.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial014 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZIPCode",
                schema: "WSA",
                table: "Mayor");

            migrationBuilder.AddColumn<int>(
                name: "ZIPCode",
                schema: "WSA",
                table: "Municipality",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZIPCode",
                schema: "WSA",
                table: "Municipality");

            migrationBuilder.AddColumn<int>(
                name: "ZIPCode",
                schema: "WSA",
                table: "Mayor",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
