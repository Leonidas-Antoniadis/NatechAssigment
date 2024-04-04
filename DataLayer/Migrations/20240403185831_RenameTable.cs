using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Natech.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class RenameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Geologation",
                table: "Geologation");

            migrationBuilder.RenameTable(
                name: "Geologation",
                newName: "Geolocation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Geolocation",
                table: "Geolocation",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Geolocation",
                table: "Geolocation");

            migrationBuilder.RenameTable(
                name: "Geolocation",
                newName: "Geologation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Geologation",
                table: "Geologation",
                column: "Id");
        }
    }
}
