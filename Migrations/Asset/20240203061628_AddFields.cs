using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DispoDataAssistant.Migrations.Asset
{
    /// <inheritdoc />
    public partial class AddFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OperationalStatus",
                table: "ServiceNowAssets",
                newName: "Substate");

            migrationBuilder.RenameColumn(
                name: "InstallStatus",
                table: "ServiceNowAssets",
                newName: "State");

            migrationBuilder.AddColumn<string>(
                name: "LifeCycleStage",
                table: "ServiceNowAssets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifeCycleStatus",
                table: "ServiceNowAssets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Parent",
                table: "ServiceNowAssets",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LifeCycleStage",
                table: "ServiceNowAssets");

            migrationBuilder.DropColumn(
                name: "LifeCycleStatus",
                table: "ServiceNowAssets");

            migrationBuilder.DropColumn(
                name: "Parent",
                table: "ServiceNowAssets");

            migrationBuilder.RenameColumn(
                name: "Substate",
                table: "ServiceNowAssets",
                newName: "OperationalStatus");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "ServiceNowAssets",
                newName: "InstallStatus");
        }
    }
}
