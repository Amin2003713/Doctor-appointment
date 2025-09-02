using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Endpoints.Migrations
{
    /// <inheritdoc />
    public partial class sadasthjry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenericName",
                table: "PrescriptionItems");

            migrationBuilder.AlterColumn<string>(
                name: "Instructions",
                table: "PrescriptionItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Frequency",
                table: "PrescriptionItems",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "PrescriptionItems",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DrugName",
                table: "PrescriptionItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Dosage",
                table: "PrescriptionItems",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "DrugId",
                table: "PrescriptionItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Drugs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GenericName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Form = table.Column<int>(type: "int", nullable: false),
                    Route = table.Column<int>(type: "int", nullable: false),
                    StrengthValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StrengthUnit = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ConcentrationText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RxClass = table.Column<int>(type: "int", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drugs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrugSynonyms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrugId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugSynonyms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrugSynonyms_Drugs_DrugId",
                        column: x => x.DrugId,
                        principalTable: "Drugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drugs_BrandName_Form_StrengthValue_StrengthUnit",
                table: "Drugs",
                columns: new[] { "BrandName", "Form", "StrengthValue", "StrengthUnit" });

            migrationBuilder.CreateIndex(
                name: "IX_Drugs_GenericName_BrandName",
                table: "Drugs",
                columns: new[] { "GenericName", "BrandName" });

            migrationBuilder.CreateIndex(
                name: "IX_Drugs_IsActive",
                table: "Drugs",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_DrugSynonyms_DrugId_Text",
                table: "DrugSynonyms",
                columns: new[] { "DrugId", "Text" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugSynonyms");

            migrationBuilder.DropTable(
                name: "Drugs");

            migrationBuilder.DropColumn(
                name: "DrugId",
                table: "PrescriptionItems");

            migrationBuilder.AlterColumn<string>(
                name: "Instructions",
                table: "PrescriptionItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Frequency",
                table: "PrescriptionItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "PrescriptionItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "DrugName",
                table: "PrescriptionItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Dosage",
                table: "PrescriptionItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AddColumn<string>(
                name: "GenericName",
                table: "PrescriptionItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
