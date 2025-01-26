using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTBlockchain.Migrations
{
    /// <inheritdoc />
    public partial class addedisvalid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "Blocks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Blocks");
        }
    }
}
