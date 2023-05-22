using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShopAlot.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial009 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TownshipFiscalOfficer",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TermEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TermStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TownshipId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TownshipFiscalOfficer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TownshipFiscalOfficer_Township_TownshipId",
                        column: x => x.TownshipId,
                        principalSchema: "WSA",
                        principalTable: "Township",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TownshipFiscalOfficer_TownshipId",
                schema: "WSA",
                table: "TownshipFiscalOfficer",
                column: "TownshipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TownshipFiscalOfficer",
                schema: "WSA");
        }
    }
}
