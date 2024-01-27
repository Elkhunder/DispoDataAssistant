using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DispoDataAssistant.Migrations
{
    /// <inheritdoc />
    public partial class AddIntegrationGeneralIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GeneralId",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IntegrationsId",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneralId",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "IntegrationsId",
                table: "Settings");
        }
    }
}
