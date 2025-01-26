using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTBlockchain.Migrations
{
    /// <inheritdoc />
    public partial class addedbloicknumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlockNumber",
                table: "Blocks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockNumber",
                table: "Blocks");
        }
    }
}
