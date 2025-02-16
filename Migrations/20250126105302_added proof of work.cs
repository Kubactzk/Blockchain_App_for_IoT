﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTBlockchain.Migrations
{
    /// <inheritdoc />
    public partial class addedproofofwork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Nonce",
                table: "Blocks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nonce",
                table: "Blocks");
        }
    }
}
