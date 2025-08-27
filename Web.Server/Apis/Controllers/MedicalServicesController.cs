using Api.Endpoints.Context;
using Api.Endpoints.Dtos.Services;
using Api.Endpoints.Models.MediaclService;
using Api.Endpoints.Models.Apointments;
using Api.Endpoints.Models.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Controllers;

[ApiController]
[Route("api/services")]
[Authorize]
public class MedicalServicesController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ServiceResponse>>> ListServices(CancellationToken ct)
    {
        var items = await db.MedicalServices.AsNoTracking()
            .OrderBy(x => x.Title)
            .Select(s => new ServiceResponse
            {
                Id = s.Id,
                Code = s.Code,
                Title = s.Title,
                Description = s.Description,
                PriceAmount = s.Price.Amount,
                PriceCurrency = s.Price.Currency,
                VisitMinutes = s.VisitMinutes,
                IsActive = s.IsActive
            })
            .ToListAsync(ct);

        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServiceResponse>> GetById(Guid id, CancellationToken ct)
    {
        var s = await db.MedicalServices.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (s is null) return NotFound();

        return Ok(new ServiceResponse
        {
            Id = s.Id,
            Code = s.Code,
            Title = s.Title,
            Description = s.Description,
            PriceAmount = s.Price.Amount,
            PriceCurrency = s.Price.Currency,
            VisitMinutes = s.VisitMinutes,
            IsActive = s.IsActive
        });
    }

    [Authorize(Roles = "Doctor")]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] UpsertServiceRequest body, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(body.Code))  return BadRequest("Code is required.");
        if (string.IsNullOrWhiteSpace(body.Title)) return BadRequest("Title is required.");

        var code = body.Code.Trim();
        var title = body.Title.Trim();

        // case-insensitive uniqueness on Code
        var exists = await db.MedicalServices
            .AnyAsync(x => x.Code.ToLower() == code.ToLower(), ct);
        if (exists) return Conflict(new { message = "Service code already exists." });

        var entity = new MedicalService
        {
            Code = code,
            Title = title,
            Description = body.Description?.Trim(),
            Price = new Money(body.PriceAmount, string.IsNullOrWhiteSpace(body.PriceCurrency) ? "IRR" : body.PriceCurrency),
            VisitMinutes = body.VisitMinutes,
            IsActive = body.IsActive
        };

        db.MedicalServices.Add(entity);
        await db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.Id);
    }

    [Authorize(Roles = "Doctor")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpsertServiceRequest body, CancellationToken ct)
    {
        var entity = await db.MedicalServices.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return NotFound();

        if (string.IsNullOrWhiteSpace(body.Code))  return BadRequest("Code is required.");
        if (string.IsNullOrWhiteSpace(body.Title)) return BadRequest("Title is required.");

        var newCode = body.Code.Trim();
        var newTitle = body.Title.Trim();

        if (!newCode.Equals(entity.Code, StringComparison.OrdinalIgnoreCase))
        {
            var exists = await db.MedicalServices
                .AnyAsync(x => x.Code.ToLower() == newCode.ToLower() && x.Id != id, ct);
            if (exists) return Conflict(new { message = "Service code already exists." });
        }

        entity.Code = newCode;
        entity.Title = newTitle;
        entity.Description = body.Description?.Trim();
        entity.Price = new Money(body.PriceAmount, string.IsNullOrWhiteSpace(body.PriceCurrency) ? "IRR" : body.PriceCurrency);
        entity.VisitMinutes = body.VisitMinutes;
        entity.IsActive = body.IsActive;

        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    [Authorize(Roles = "Doctor")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var entity = await db.MedicalServices.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return NotFound();

        // prevent deletion if referenced by appointments
        var inUse = await db.Appointments.AsNoTracking()
            .AnyAsync(a => a.ServiceId == id && a.Status != AppointmentStatus.Cancelled, ct);
        if (inUse) return Conflict(new { message = "Cannot delete: service is used by existing appointments." });

        db.MedicalServices.Remove(entity);
        await db.SaveChangesAsync(ct);
        return NoContent();
    }
}
