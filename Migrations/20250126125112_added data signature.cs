using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTBlockchain.Migrations
{
    /// <inheritdoc />
    public partial class addeddatasignature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataSignature",
                table: "Blocks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataSignature",
                table: "Blocks");
        }
    }
}
