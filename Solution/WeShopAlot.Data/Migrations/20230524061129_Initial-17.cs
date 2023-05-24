using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShopAlot.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MunicipalIncomeTaxRate",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal (5,3)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalIncomeTaxRate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MunicipalIncomeTaxRate_Municipality_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalSchema: "WSA",
                        principalTable: "Municipality",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalIncomeTaxRate_MunicipalityId",
                schema: "WSA",
                table: "MunicipalIncomeTaxRate",
                column: "MunicipalityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MunicipalIncomeTaxRate",
                schema: "WSA");
        }
    }
}
