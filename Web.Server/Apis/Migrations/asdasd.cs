#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Endpoints.Migrations;


public partial class asdasd : Migration
{
    
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "Token",
            "AspNetUsers");
    }

    
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