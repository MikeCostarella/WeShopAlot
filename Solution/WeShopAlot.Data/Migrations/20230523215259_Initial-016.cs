using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShopAlot.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial016 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TermEndDate",
                schema: "WSA",
                table: "TownshipTrustee",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "WSA",
                table: "TownshipTrustee",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TermEndDate",
                schema: "WSA",
                table: "TownshipFiscalOfficer",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "WSA",
                table: "TownshipFiscalOfficer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "WSA",
                table: "MunicipalTreasurer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "WSA",
                table: "MunicipalLawDirector",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "WSA",
                table: "MunicipalCouncilPresident",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "WSA",
                table: "MunicipalAuditor",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "WSA",
                table: "Mayor",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MunicipalCouncilMember",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TermEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TermStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalCouncilMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MunicipalCouncilMember_Municipality_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalSchema: "WSA",
                        principalTable: "Municipality",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalCouncilMember_MunicipalityId",
                schema: "WSA",
                table: "MunicipalCouncilMember",
                column: "MunicipalityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MunicipalCouncilMember",
                schema: "WSA");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "WSA",
                table: "TownshipTrustee");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "WSA",
                table: "TownshipFiscalOfficer");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "WSA",
                table: "MunicipalTreasurer");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "WSA",
                table: "MunicipalLawDirector");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "WSA",
                table: "MunicipalCouncilPresident");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "WSA",
                table: "MunicipalAuditor");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "WSA",
                table: "Mayor");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TermEndDate",
                schema: "WSA",
                table: "TownshipTrustee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TermEndDate",
                schema: "WSA",
                table: "TownshipFiscalOfficer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
