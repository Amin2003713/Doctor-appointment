using System.Security.Claims;
using Api.Endpoints.Context;
using Api.Endpoints.Dtos.Clinic;
using Api.Endpoints.Dtos.doctor;
using Api.Endpoints.Dtos.Schedules;
using Api.Endpoints.Dtos.Services;
using Api.Endpoints.Models.Apointments;
using Api.Endpoints.Models.Clinic;
using Api.Endpoints.Models.Doctores;
using Api.Endpoints.Models.MediaclService;
using Api.Endpoints.Models.Schedule;
using Api.Endpoints.Models.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


// NOTE: assumes you already have these domain models & DbContext from earlier:
// - ClinicSettings, PaymentMethods
// - MedicalService (with Money Price), WorkSchedule, WorkingDay, SpecialDateOverride, TimeRange
// - AppDbContext configured with TimeOnly<->TimeSpan converters for TimeRange

namespace Api.Endpoints.Controllers;

[ApiController]
[Route("api/clinic")]
[Authorize]
public class ClinicController (
    AppDbContext db
) : ControllerBase
{

    [HttpGet("settings")]
    public async Task<ActionResult<ClinicSettingsResponse>> GetSettings(CancellationToken ct)
    {
        var s = await db.ClinicSettings.AsNoTracking().FirstOrDefaultAsync(ct) ?? new ClinicSettings(); // default empty if not set yet

        return Ok(new ClinicSettingsResponse
        {
            Name = s.Name,
            Address = s.Address,
            PhoneNumber = s.PhoneNumber,
            Email = s.Email,
            Latitude = s.Latitude,
            Longitude = s.Longitude,
            Notes = s.Notes,
            AcceptedPayments = s.AcceptedPayments,
            DefaultVisitMinutes = s.DefaultVisitMinutes,
            BufferBetweenVisitsMinutes = s.BufferBetweenVisitsMinutes
        });
    }

    [Authorize(Roles = "Doctor")]
    [HttpPut("settings")]
    public async Task<IActionResult> UpdateSettings([FromBody] UpdateClinicSettingsRequest body, CancellationToken ct)
    {

        var s = await db.ClinicSettings.FirstOrDefaultAsync(ct);

        if (s is null)
            return Ok();

        s.Name = body.Name;
        s.Address = body.Address;
        s.PhoneNumber = body.PhoneNumber;
        s.Email = body.Email;
        s.Latitude = body.Latitude;
        s.Longitude = body.Longitude;
        s.Notes = body.Notes;
        s.AcceptedPayments = body.AcceptedPayments;
        s.DefaultVisitMinutes = body.DefaultVisitMinutes;
        s.BufferBetweenVisitsMinutes = body.BufferBetweenVisitsMinutes;

        await db.SaveChangesAsync(ct);
        return NoContent();
    }

   

   


   
}