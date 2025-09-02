using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Endpoints.Migrations
{
    
    public partial class asd : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PatientFullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Start = table.Column<TimeOnly>(type: "time", nullable: false),
                    End = table.Column<TimeOnly>(type: "time", nullable: false),
                    PriceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClinicSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    AcceptedPayments = table.Column<int>(type: "int", nullable: false),
                    DefaultVisitMinutes = table.Column<int>(type: "int", nullable: false),
                    BufferBetweenVisitsMinutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoctorProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Education = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Languages = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhatsApp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price_Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    VisitMinutes = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecialDateOverrides",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    WorkScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialDateOverrides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialDateOverrides_WorkSchedules_WorkScheduleId",
                        column: x => x.WorkScheduleId,
                        principalTable: "WorkSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkingDay",
                columns: table => new
                {
                    WorkScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Closed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingDay", x => new { x.WorkScheduleId, x.Id });
                    table.ForeignKey(
                        name: "FK_WorkingDay_WorkSchedules_WorkScheduleId",
                        column: x => x.WorkScheduleId,
                        principalTable: "WorkSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialDateOverrides_Intervals",
                columns: table => new
                {
                    SpecialDateOverrideId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<TimeSpan>(type: "time", nullable: false),
                    To = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialDateOverrides_Intervals", x => new { x.SpecialDateOverrideId, x.Id });
                    table.ForeignKey(
                        name: "FK_SpecialDateOverrides_Intervals_SpecialDateOverrides_SpecialDateOverrideId",
                        column: x => x.SpecialDateOverrideId,
                        principalTable: "SpecialDateOverrides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkingDay_Breaks",
                columns: table => new
                {
                    WorkingDayWorkScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkingDayId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<TimeSpan>(type: "time", nullable: false),
                    To = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingDay_Breaks", x => new { x.WorkingDayWorkScheduleId, x.WorkingDayId, x.Id });
                    table.ForeignKey(
                        name: "FK_WorkingDay_Breaks_WorkingDay_WorkingDayWorkScheduleId_WorkingDayId",
                        columns: x => new { x.WorkingDayWorkScheduleId, x.WorkingDayId },
                        principalTable: "WorkingDay",
                        principalColumns: new[] { "WorkScheduleId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkingDay_Intervals",
                columns: table => new
                {
                    WorkingDayWorkScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkingDayId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<TimeSpan>(type: "time", nullable: false),
                    To = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingDay_Intervals", x => new { x.WorkingDayWorkScheduleId, x.WorkingDayId, x.Id });
                    table.ForeignKey(
                        name: "FK_WorkingDay_Intervals_WorkingDay_WorkingDayWorkScheduleId_WorkingDayId",
                        columns: x => new { x.WorkingDayWorkScheduleId, x.WorkingDayId },
                        principalTable: "WorkingDay",
                        principalColumns: new[] { "WorkScheduleId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalServices_Code",
                table: "MedicalServices",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialDateOverrides_WorkScheduleId",
                table: "SpecialDateOverrides",
                column: "WorkScheduleId");
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "ClinicSettings");

            migrationBuilder.DropTable(
                name: "DoctorProfiles");

            migrationBuilder.DropTable(
                name: "MedicalServices");

            migrationBuilder.DropTable(
                name: "SpecialDateOverrides_Intervals");

            migrationBuilder.DropTable(
                name: "WorkingDay_Breaks");

            migrationBuilder.DropTable(
                name: "WorkingDay_Intervals");

            migrationBuilder.DropTable(
                name: "SpecialDateOverrides");

            migrationBuilder.DropTable(
                name: "WorkingDay");

            migrationBuilder.DropTable(
                name: "WorkSchedules");
        }
    }
}
