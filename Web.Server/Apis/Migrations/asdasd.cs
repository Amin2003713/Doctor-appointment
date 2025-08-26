#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Endpoints.Migrations;

/// <inheritdoc />
public partial class asdasd : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "Token",
            "AspNetUsers");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            "Token",
            "AspNetUsers",
            "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }
}