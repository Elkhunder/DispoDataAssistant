using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DispoDataAssistant.Migrations.ServiceNowAsset
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceNowAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SysId = table.Column<string>(type: "TEXT", nullable: true),
                    TabId = table.Column<int>(type: "INTEGER", nullable: false),
                    TabName = table.Column<string>(type: "TEXT", nullable: true),
                    AssetTag = table.Column<string>(type: "TEXT", nullable: true),
                    Manufacturer = table.Column<string>(type: "TEXT", nullable: true),
                    Model = table.Column<string>(type: "TEXT", nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: true),
                    SerialNumber = table.Column<string>(type: "TEXT", nullable: true),
                    OperationalStatus = table.Column<string>(type: "TEXT", nullable: true),
                    InstallStatus = table.Column<string>(type: "TEXT", nullable: true),
                    LastUpdated = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceNowAssets", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceNowAssets");
        }
    }
}
