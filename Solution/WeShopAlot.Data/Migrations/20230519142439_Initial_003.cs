using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShopAlot.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial_003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_County_State_StateId",
                schema: "WSA",
                table: "County");

            migrationBuilder.DropForeignKey(
                name: "FK_Municipality_State_StateId",
                schema: "WSA",
                table: "Municipality");

            migrationBuilder.DropTable(
                name: "State",
                schema: "WSA");

            migrationBuilder.CreateTable(
                name: "StateProvince",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Abbreviation = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateProvince", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StateProvince_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "WSA",
                        principalTable: "Country",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StateProvince_CountryId",
                schema: "WSA",
                table: "StateProvince",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_County_StateProvince_StateId",
                schema: "WSA",
                table: "County",
                column: "StateId",
                principalSchema: "WSA",
                principalTable: "StateProvince",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Municipality_StateProvince_StateId",
                schema: "WSA",
                table: "Municipality",
                column: "StateId",
                principalSchema: "WSA",
                principalTable: "StateProvince",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_County_StateProvince_StateId",
                schema: "WSA",
                table: "County");

            migrationBuilder.DropForeignKey(
                name: "FK_Municipality_StateProvince_StateId",
                schema: "WSA",
                table: "Municipality");

            migrationBuilder.DropTable(
                name: "StateProvince",
                schema: "WSA");

            migrationBuilder.CreateTable(
                name: "State",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                    table.ForeignKey(
                        name: "FK_State_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "WSA",
                        principalTable: "Country",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_State_CountryId",
                schema: "WSA",
                table: "State",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_County_State_StateId",
                schema: "WSA",
                table: "County",
                column: "StateId",
                principalSchema: "WSA",
                principalTable: "State",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Municipality_State_StateId",
                schema: "WSA",
                table: "Municipality",
                column: "StateId",
                principalSchema: "WSA",
                principalTable: "State",
                principalColumn: "Id");
        }
    }
}
