using Api.Endpoints.Context;
using Api.Endpoints.Dtos.doctor;
using Api.Endpoints.Models.Doctores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Controllers;

[ApiController]
[Route("api/doctor")]
[Authorize]
public sealed class DoctorController(AppDbContext db) : ControllerBase
{
    
    private static readonly Guid SingletonId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    private async Task<DoctorProfile> EnsureProfileAsync(CancellationToken ct)
    {
        var p = await db.DoctorProfiles.FirstOrDefaultAsync(ct);
        if (p is not null) return p;

        p = new DoctorProfile { Id = SingletonId, Title = "MD" };
        db.DoctorProfiles.Add(p);
        await db.SaveChangesAsync(ct);
        return p;
    }

    [HttpGet("profile")]
    [AllowAnonymous] 
    public async Task<ActionResult<DoctorProfileResponse>> GetDoctorProfile(CancellationToken ct)
    {
        var p = await db.DoctorProfiles.AsNoTracking().FirstOrDefaultAsync(ct)
                ?? await EnsureProfileAsync(ct);

        return Ok(new DoctorProfileResponse
        {
            FullName = p.FullName ?? "",
            Title = p.Title ?? "",
            Biography = p.Biography ?? "",
            Specialties = p.Specialties ?? Array.Empty<string>(),
            Education = p.Education ?? Array.Empty<string>(),
            Languages = p.Languages ?? Array.Empty<string>(),
            YearsOfExperience = p.YearsOfExperience,
            PhotoUrl = p.PhotoUrl,
            Website = p.Website,
            Instagram = p.Instagram,
            LinkedIn = p.LinkedIn,
            WhatsApp = p.WhatsApp
        });
    }

    [HttpPut("profile")]
    [Authorize(Roles = "Doctor,Secretary")]
    public async Task<IActionResult> UpsertDoctorProfile([FromBody] UpsertDoctorProfileRequest body, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(body.FullName))
            return BadRequest("FullName is required.");

        var p = await db.DoctorProfiles.FirstOrDefaultAsync(ct) ?? new DoctorProfile { Id = SingletonId };

        p.FullName          = body.FullName.Trim();
        p.Title             = (body.Title ?? "").Trim();
        p.Biography         = body.Biography?.Trim() ?? "";
        p.Specialties       = body.Specialties ?? Array.Empty<string>();
        p.Education         = body.Education ?? Array.Empty<string>();
        p.Languages         = body.Languages ?? Array.Empty<string>();
        p.YearsOfExperience = Math.Max(0, body.YearsOfExperience);
        p.PhotoUrl          = string.IsNullOrWhiteSpace(body.PhotoUrl) ? null : body.PhotoUrl.Trim();
        p.Website           = string.IsNullOrWhiteSpace(body.Website) ? null : body.Website.Trim();
        p.Instagram         = string.IsNullOrWhiteSpace(body.Instagram) ? null : body.Instagram.Trim();
        p.LinkedIn          = string.IsNullOrWhiteSpace(body.LinkedIn) ? null : body.LinkedIn.Trim();
        p.WhatsApp          = string.IsNullOrWhiteSpace(body.WhatsApp) ? null : body.WhatsApp.Trim();

        if (db.Entry(p).State == EntityState.Detached)
            db.DoctorProfiles.Add(p);

        await db.SaveChangesAsync(ct);
        return NoContent();
    }
}
