using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WeShopAlot.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial021 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Country_CountryId",
                schema: "WSA",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Address_County_CountyId",
                schema: "WSA",
                table: "Address");

            migrationBuilder.DropTable(
                name: "GovernmentScope",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "Individual",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "Mayor",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "MunicipalAuditor",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "MunicipalCouncilMember",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "MunicipalCouncilPresident",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "MunicipalIncomeTaxRate",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "MunicipalityCounty",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "MunicipalityTaxAgencyRelationship",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "MunicipalLawDirector",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "MunicipalTreasurer",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "Precinct",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "TownshipFiscalOfficer",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "TownshipTrustee",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "TaxAgencyMembershipType",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "Municipality",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "Township",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "MunicipalIncomeTaxManagementAgency",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "MunicipalityType",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "County",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "StateProvince",
                schema: "WSA");

            migrationBuilder.DropTable(
                name: "Country",
                schema: "WSA");

            migrationBuilder.DropIndex(
                name: "IX_Address_CountryId",
                schema: "WSA",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_CountyId",
                schema: "WSA",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "CountryId",
                schema: "WSA",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "CountyId",
                schema: "WSA",
                table: "Address");

            migrationBuilder.AddColumn<string>(
                name: "State",
                schema: "WSA",
                table: "Address",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                schema: "WSA",
                table: "Address");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                schema: "WSA",
                table: "Address",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountyId",
                schema: "WSA",
                table: "Address",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Country",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Abbreviation = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GovernmentScope",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovernmentScope", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Individual",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Individual", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MunicipalIncomeTaxManagementAgency",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Abbreviation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InternalId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalIncomeTaxManagementAgency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MunicipalityType",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalityType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxAgencyMembershipType",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxAgencyMembershipType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StateProvince",
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
                    table.PrimaryKey("PK_StateProvince", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StateProvince_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "WSA",
                        principalTable: "Country",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "County",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateProvinceId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_County", x => x.Id);
                    table.ForeignKey(
                        name: "FK_County_StateProvince_StateProvinceId",
                        column: x => x.StateProvinceId,
                        principalSchema: "WSA",
                        principalTable: "StateProvince",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Municipality",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MunicipalIncomeTaxManagementAgencyId = table.Column<int>(type: "int", nullable: true),
                    MunicipalityTypeId = table.Column<int>(type: "int", nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    Census2000 = table.Column<int>(type: "int", nullable: false),
                    Census2010 = table.Column<int>(type: "int", nullable: false),
                    Census2020 = table.Column<int>(type: "int", nullable: false),
                    FormOfGovernment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCollectedByRITA = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MailingAddressLine1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MailingAddressLine2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    YearIncorporated = table.Column<int>(type: "int", nullable: false),
                    ZIPCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipality", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Municipality_MunicipalIncomeTaxManagementAgency_MunicipalIncomeTaxManagementAgencyId",
                        column: x => x.MunicipalIncomeTaxManagementAgencyId,
                        principalSchema: "WSA",
                        principalTable: "MunicipalIncomeTaxManagementAgency",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Municipality_MunicipalityType_MunicipalityTypeId",
                        column: x => x.MunicipalityTypeId,
                        principalSchema: "WSA",
                        principalTable: "MunicipalityType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Municipality_StateProvince_StateId",
                        column: x => x.StateId,
                        principalSchema: "WSA",
                        principalTable: "StateProvince",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Precinct",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountyId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MediaMarket = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Precinct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Precinct_County_CountyId",
                        column: x => x.CountyId,
                        principalSchema: "WSA",
                        principalTable: "County",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Township",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountyId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MailingAddressCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MailingAddressLine1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MailingAddressLine2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MailingAddressZIPCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WebSiteUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Township", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Township_County_CountyId",
                        column: x => x.CountyId,
                        principalSchema: "WSA",
                        principalTable: "County",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Mayor",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TermEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TermStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "MunicipalAuditor",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TermEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TermStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
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
                name: "MunicipalCouncilMember",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "MunicipalCouncilPresident",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TermEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TermStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
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
                name: "MunicipalIncomeTaxRate",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal (5,3)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "MunicipalityCounty",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountyId = table.Column<int>(type: "int", nullable: false),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalityCounty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MunicipalityCounty_County_CountyId",
                        column: x => x.CountyId,
                        principalSchema: "WSA",
                        principalTable: "County",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MunicipalityCounty_Municipality_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalSchema: "WSA",
                        principalTable: "Municipality",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MunicipalityTaxAgencyRelationship",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MunicipalIncomeTaxManagementAgencyId = table.Column<int>(type: "int", nullable: false),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    TaxAgencyMembershipTypeId = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "MunicipalLawDirector",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TermEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TermStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
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
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TermEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TermStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "TownshipFiscalOfficer",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TownshipId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TermEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TermStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "TownshipTrustee",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TownshipId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TermEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TermStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TownshipTrustee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TownshipTrustee_Township_TownshipId",
                        column: x => x.TownshipId,
                        principalSchema: "WSA",
                        principalTable: "Township",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "WSA",
                table: "GovernmentScope",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, "Federal", false, "Federal" },
                    { 2, "State", false, "State" },
                    { 3, "County", false, "County" },
                    { 4, "Township", false, "Township" },
                    { 5, "Municipal", false, "Municipal" }
                });

            migrationBuilder.InsertData(
                schema: "WSA",
                table: "MunicipalityType",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, "City", false, "City" },
                    { 2, "Village", false, "Village" }
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
                name: "IX_Address_CountryId",
                schema: "WSA",
                table: "Address",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_CountyId",
                schema: "WSA",
                table: "Address",
                column: "CountyId");

            migrationBuilder.CreateIndex(
                name: "IX_County_StateProvinceId",
                schema: "WSA",
                table: "County",
                column: "StateProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Mayor_MunicipalityId",
                schema: "WSA",
                table: "Mayor",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalAuditor_MunicipalityId",
                schema: "WSA",
                table: "MunicipalAuditor",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalCouncilMember_MunicipalityId",
                schema: "WSA",
                table: "MunicipalCouncilMember",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalCouncilPresident_MunicipalityId",
                schema: "WSA",
                table: "MunicipalCouncilPresident",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalIncomeTaxRate_MunicipalityId",
                schema: "WSA",
                table: "MunicipalIncomeTaxRate",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipality_MunicipalIncomeTaxManagementAgencyId",
                schema: "WSA",
                table: "Municipality",
                column: "MunicipalIncomeTaxManagementAgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipality_MunicipalityTypeId",
                schema: "WSA",
                table: "Municipality",
                column: "MunicipalityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipality_StateId",
                schema: "WSA",
                table: "Municipality",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityCounty_CountyId",
                schema: "WSA",
                table: "MunicipalityCounty",
                column: "CountyId");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityCounty_MunicipalityId",
                schema: "WSA",
                table: "MunicipalityCounty",
                column: "MunicipalityId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Precinct_CountyId",
                schema: "WSA",
                table: "Precinct",
                column: "CountyId");

            migrationBuilder.CreateIndex(
                name: "IX_StateProvince_CountryId",
                schema: "WSA",
                table: "StateProvince",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Township_CountyId",
                schema: "WSA",
                table: "Township",
                column: "CountyId");

            migrationBuilder.CreateIndex(
                name: "IX_TownshipFiscalOfficer_TownshipId",
                schema: "WSA",
                table: "TownshipFiscalOfficer",
                column: "TownshipId");

            migrationBuilder.CreateIndex(
                name: "IX_TownshipTrustee_TownshipId",
                schema: "WSA",
                table: "TownshipTrustee",
                column: "TownshipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Country_CountryId",
                schema: "WSA",
                table: "Address",
                column: "CountryId",
                principalSchema: "WSA",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Address_County_CountyId",
                schema: "WSA",
                table: "Address",
                column: "CountyId",
                principalSchema: "WSA",
                principalTable: "County",
                principalColumn: "Id");
        }
    }
}
