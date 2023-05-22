using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShopAlot.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial010 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MailingAddressCity",
                schema: "WSA",
                table: "Township",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailingAddressLine1",
                schema: "WSA",
                table: "Township",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailingAddressLine2",
                schema: "WSA",
                table: "Township",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailingAddressZIPCode",
                schema: "WSA",
                table: "Township",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MailingAddressCity",
                schema: "WSA",
                table: "Township");

            migrationBuilder.DropColumn(
                name: "MailingAddressLine1",
                schema: "WSA",
                table: "Township");

            migrationBuilder.DropColumn(
                name: "MailingAddressLine2",
                schema: "WSA",
                table: "Township");

            migrationBuilder.DropColumn(
                name: "MailingAddressZIPCode",
                schema: "WSA",
                table: "Township");
        }
    }
}
