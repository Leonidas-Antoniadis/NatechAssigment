using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Natech.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class RemovePropertyFinished : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finished",
                table: "Batches");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "Batches",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
