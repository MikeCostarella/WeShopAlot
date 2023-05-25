using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShopAlot.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial019 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MunicipalIncomeTaxManagementAgencyId",
                schema: "WSA",
                table: "Municipality",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MunicipalIncomeTaxManagementAgency",
                schema: "WSA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Abbreviation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InternalId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalIncomeTaxManagementAgency", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Municipality_MunicipalIncomeTaxManagementAgencyId",
                schema: "WSA",
                table: "Municipality",
                column: "MunicipalIncomeTaxManagementAgencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Municipality_MunicipalIncomeTaxManagementAgency_MunicipalIncomeTaxManagementAgencyId",
                schema: "WSA",
                table: "Municipality",
                column: "MunicipalIncomeTaxManagementAgencyId",
                principalSchema: "WSA",
                principalTable: "MunicipalIncomeTaxManagementAgency",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Municipality_MunicipalIncomeTaxManagementAgency_MunicipalIncomeTaxManagementAgencyId",
                schema: "WSA",
                table: "Municipality");

            migrationBuilder.DropTable(
                name: "MunicipalIncomeTaxManagementAgency",
                schema: "WSA");

            migrationBuilder.DropIndex(
                name: "IX_Municipality_MunicipalIncomeTaxManagementAgencyId",
                schema: "WSA",
                table: "Municipality");

            migrationBuilder.DropColumn(
                name: "MunicipalIncomeTaxManagementAgencyId",
                schema: "WSA",
                table: "Municipality");
        }
    }
}
