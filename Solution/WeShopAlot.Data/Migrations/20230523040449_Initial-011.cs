using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShopAlot.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial011 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Census2000",
                schema: "WSA",
                table: "Municipality",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Census2010",
                schema: "WSA",
                table: "Municipality",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Census2020",
                schema: "WSA",
                table: "Municipality",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FormOfGovernment",
                schema: "WSA",
                table: "Municipality",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailingAddressLine1",
                schema: "WSA",
                table: "Municipality",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailingAddressLine2",
                schema: "WSA",
                table: "Municipality",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telephone",
                schema: "WSA",
                table: "Municipality",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                schema: "WSA",
                table: "Municipality",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearIncorporated",
                schema: "WSA",
                table: "Municipality",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Mayor",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    TermEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TermStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mayor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mayor_Municipality_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalSchema: "WSA",
                        principalTable: "Municipality",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mayor_MunicipalityId",
                schema: "WSA",
                table: "Mayor",
                column: "MunicipalityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mayor",
                schema: "WSA");

            migrationBuilder.DropColumn(
                name: "Census2000",
                schema: "WSA",
                table: "Municipality");

            migrationBuilder.DropColumn(
                name: "Census2010",
                schema: "WSA",
                table: "Municipality");

            migrationBuilder.DropColumn(
                name: "Census2020",
                schema: "WSA",
                table: "Municipality");

            migrationBuilder.DropColumn(
                name: "FormOfGovernment",
                schema: "WSA",
                table: "Municipality");

            migrationBuilder.DropColumn(
                name: "MailingAddressLine1",
                schema: "WSA",
                table: "Municipality");

            migrationBuilder.DropColumn(
                name: "MailingAddressLine2",
                schema: "WSA",
                table: "Municipality");

            migrationBuilder.DropColumn(
                name: "Telephone",
                schema: "WSA",
                table: "Municipality");

            migrationBuilder.DropColumn(
                name: "Website",
                schema: "WSA",
                table: "Municipality");

            migrationBuilder.DropColumn(
                name: "YearIncorporated",
                schema: "WSA",
                table: "Municipality");
        }
    }
}
