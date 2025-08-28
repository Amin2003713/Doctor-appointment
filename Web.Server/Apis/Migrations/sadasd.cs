#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Endpoints.Migrations;


public partial class sadasd : Migration
{
    
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            "Address",
            "AspNetUsers",
            "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            "FirstName",
            "AspNetUsers",
            "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            "LastName",
            "AspNetUsers",
            "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            "Profile",
            "AspNetUsers",
            "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            "Token",
            "AspNetUsers",
            "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "Address",
            "AspNetUsers");

        migrationBuilder.DropColumn(
            "FirstName",
            "AspNetUsers");

        migrationBuilder.DropColumn(
            "LastName",
            "AspNetUsers");

        migrationBuilder.DropColumn(
            "Profile",
            "AspNetUsers");

        migrationBuilder.DropColumn(
            "Token",
            "AspNetUsers");
    }
}