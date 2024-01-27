using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DispoDataAssistant.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_General_Settings_SettingsId",
                table: "General");

            migrationBuilder.DropForeignKey(
                name: "FK_Integrations_Settings_SettingsId",
                table: "Integrations");

            migrationBuilder.DropColumn(
                name: "GeneralId",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "IntegrationsId",
                table: "Settings");

            migrationBuilder.AlterColumn<int>(
                name: "SettingsId",
                table: "Integrations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SettingsId",
                table: "General",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_General_Settings_SettingsId",
                table: "General",
                column: "SettingsId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Integrations_Settings_SettingsId",
                table: "Integrations",
                column: "SettingsId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_General_Settings_SettingsId",
                table: "General");

            migrationBuilder.DropForeignKey(
                name: "FK_Integrations_Settings_SettingsId",
                table: "Integrations");

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

            migrationBuilder.AlterColumn<int>(
                name: "SettingsId",
                table: "Integrations",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "SettingsId",
                table: "General",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_General_Settings_SettingsId",
                table: "General",
                column: "SettingsId",
                principalTable: "Settings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Integrations_Settings_SettingsId",
                table: "Integrations",
                column: "SettingsId",
                principalTable: "Settings",
                principalColumn: "Id");
        }
    }
}
