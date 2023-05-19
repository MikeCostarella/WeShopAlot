using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShopAlot.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial_004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_County_StateProvince_StateId",
                schema: "WSA",
                table: "County");

            migrationBuilder.RenameColumn(
                name: "StateId",
                schema: "WSA",
                table: "County",
                newName: "StateProvinceId");

            migrationBuilder.RenameIndex(
                name: "IX_County_StateId",
                schema: "WSA",
                table: "County",
                newName: "IX_County_StateProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_County_StateProvince_StateProvinceId",
                schema: "WSA",
                table: "County",
                column: "StateProvinceId",
                principalSchema: "WSA",
                principalTable: "StateProvince",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_County_StateProvince_StateProvinceId",
                schema: "WSA",
                table: "County");

            migrationBuilder.RenameColumn(
                name: "StateProvinceId",
                schema: "WSA",
                table: "County",
                newName: "StateId");

            migrationBuilder.RenameIndex(
                name: "IX_County_StateProvinceId",
                schema: "WSA",
                table: "County",
                newName: "IX_County_StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_County_StateProvince_StateId",
                schema: "WSA",
                table: "County",
                column: "StateId",
                principalSchema: "WSA",
                principalTable: "StateProvince",
                principalColumn: "Id");
        }
    }
}
