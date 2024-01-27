using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DispoDataAssistant.Migrations.Asset
{
    /// <inheritdoc />
    public partial class AddTabIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "Tabs",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "Tabs");
        }
    }
}
