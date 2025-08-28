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
public sealed class DoctorController(
    AppDbContext db
) : ControllerBase
{
    private static readonly Guid SingletonId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    private async Task<DoctorProfile> EnsureProfileAsync(CancellationToken ct)
    {
        var p = await db.DoctorProfiles.FirstOrDefaultAsync(ct);
        if (p is not null) return p;

        p = new DoctorProfile
            {
                Id = Guid.Parse("8F7B5F7F-2E3F-4A0C-9E8E-7D8E6E5C4B3A"),
                FullName = "دکتر ساسان رحیمی",
                Title = "متخصص قلب و عروق (MD, FACC)",
                Biography
                    = "بیش از ۱۲ سال تجربه در تشخیص و درمان اختلالات قلبی–عروقی، با تمرکز بر پیشگیری، کنترل فشارخون و درمان نارسایی قلب. ارائه‌دهنده خدمات اکوکاردیوگرافی، تست ورزش و پایش ریتم قلب.",

                Specialties = new[]
                {
                    "قلب و عروق",
                    "اکوکاردیوگرافی",
                    "کنترل فشار خون",
                    "نارسایی قلب"
                },

                Education = new[]
                {
                    "فلوشیپ قلب و عروق – دانشگاه علوم پزشکی تهران",
                    "دکترای پزشکی – دانشگاه علوم پزشکی شیراز",
                    "FACC – American College of Cardiology"
                },

                Languages = new[]
                {
                    "فارسی",
                    "انگلیسی"
                },

                YearsOfExperience = 12,
                PhotoUrl = "/images/doctors/sara-rahimi.jpg",
                Website = "rahimi-cardiology.ir",
                Instagram = "instagram.com/dr.rahimi.cardiology",
                LinkedIn = "linkedin.com/in/sara-rahimi-md",
                WhatsApp = "+989121234567"
            }
            ;

        db.DoctorProfiles.Add(p);
        await db.SaveChangesAsync(ct);
        return p;
    }

    [HttpGet("profile")]
    [AllowAnonymous]
    public async Task<ActionResult<DoctorProfileResponse>> GetDoctorProfile(CancellationToken ct)
    {
        var p = await db.DoctorProfiles.AsNoTracking().FirstOrDefaultAsync(ct) ?? await EnsureProfileAsync(ct);

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

        var p = await db.DoctorProfiles.FirstOrDefaultAsync(ct) ?? await EnsureProfileAsync(ct);

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