using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShopAlot.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial015 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MunicipalAuditor",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    TermEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TermStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalAuditor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MunicipalAuditor_Municipality_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalSchema: "WSA",
                        principalTable: "Municipality",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MunicipalCouncilPresident",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    TermEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TermStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalCouncilPresident", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MunicipalCouncilPresident_Municipality_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalSchema: "WSA",
                        principalTable: "Municipality",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MunicipalLawDirector",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    TermEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TermStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalLawDirector", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MunicipalLawDirector_Municipality_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalSchema: "WSA",
                        principalTable: "Municipality",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MunicipalTreasurer",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    TermEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TermStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalTreasurer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MunicipalTreasurer_Municipality_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalSchema: "WSA",
                        principalTable: "Municipality",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalAuditor_MunicipalityId",
                schema: "WSA",
                table: "MunicipalAuditor",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalCouncilPresident_MunicipalityId",
                schema: "WSA",
                table: "MunicipalCouncilPresident",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalLawDirector_MunicipalityId",
                schema: "WSA",
                table: "MunicipalLawDirector",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalTreasurer_MunicipalityId",
                schema: "WSA",
                table: "MunicipalTreasurer",
                column: "MunicipalityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MunicipalAuditor",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "MunicipalCouncilPresident",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "MunicipalLawDirector",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "MunicipalTreasurer",
                schema: "WSA");
        }
    }
}
