using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WeShopAlot.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial020 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxAgencyMembershipType",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxAgencyMembershipType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MunicipalityTaxAgencyRelationship",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    MunicipalIncomeTaxManagementAgencyId = table.Column<int>(type: "int", nullable: false),
                    TaxAgencyMembershipTypeId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalityTaxAgencyRelationship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MunicipalityTaxAgencyRelationship_MunicipalIncomeTaxManagementAgency_MunicipalIncomeTaxManagementAgencyId",
                        column: x => x.MunicipalIncomeTaxManagementAgencyId,
                        principalSchema: "WSA",
                        principalTable: "MunicipalIncomeTaxManagementAgency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MunicipalityTaxAgencyRelationship_Municipality_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalSchema: "WSA",
                        principalTable: "Municipality",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MunicipalityTaxAgencyRelationship_TaxAgencyMembershipType_TaxAgencyMembershipTypeId",
                        column: x => x.TaxAgencyMembershipTypeId,
                        principalSchema: "WSA",
                        principalTable: "TaxAgencyMembershipType",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "WSA",
                table: "TaxAgencyMembershipType",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, "Full", false, "Full" },
                    { 2, "Special", false, "Special" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityTaxAgencyRelationship_MunicipalIncomeTaxManagementAgencyId",
                schema: "WSA",
                table: "MunicipalityTaxAgencyRelationship",
                column: "MunicipalIncomeTaxManagementAgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityTaxAgencyRelationship_MunicipalityId",
                schema: "WSA",
                table: "MunicipalityTaxAgencyRelationship",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityTaxAgencyRelationship_TaxAgencyMembershipTypeId",
                schema: "WSA",
                table: "MunicipalityTaxAgencyRelationship",
                column: "TaxAgencyMembershipTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MunicipalityTaxAgencyRelationship",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "TaxAgencyMembershipType",
                schema: "WSA");
        }
    }
}
