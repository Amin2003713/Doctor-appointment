#nullable enable
using System.Security.Claims;
using Api.Endpoints.Context;
using Api.Endpoints.Dtos.Drugs;
using Api.Endpoints.Models.Drugs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Controllers.Drugs;

[ApiController]
[Route("api/drugs")]
[Authorize]
public class DrugsController(AppDbContext db) : ControllerBase
{
    // ---------------------------
    // CREATE
    // ---------------------------
    [Authorize(Roles = "Doctor,Secretary")] // optionally "Admin"
    [HttpPost]
    public async Task<ActionResult<DrugResponse>> Create([FromBody] UpsertDrugRequest body, CancellationToken ct)
    {
        var (ok, err) = Validate(body);
        if (!ok) return BadRequest(err);

        var entity = new Drug
        {
            BrandName = body.BrandName.Trim(),
            GenericName = body.GenericName.Trim(),
            Form = (DrugForm)body.Form,
            Route = (DrugRoute)body.Route,
            StrengthValue = body.StrengthValue,
            StrengthUnit = string.IsNullOrWhiteSpace(body.StrengthUnit) ? null : body.StrengthUnit.Trim(),
            ConcentrationText = string.IsNullOrWhiteSpace(body.ConcentrationText) ? null : body.ConcentrationText.Trim(),
            RxClass = (RxClass)body.RxClass,
            Manufacturer = string.IsNullOrWhiteSpace(body.Manufacturer) ? null : body.Manufacturer.Trim(),
            Country = string.IsNullOrWhiteSpace(body.Country) ? null : body.Country.Trim(),
            Barcode = string.IsNullOrWhiteSpace(body.Barcode) ? null : body.Barcode.Trim(),
            Tags = string.IsNullOrWhiteSpace(body.Tags) ? null : body.Tags.Trim(),
            IsActive = body.IsActive
        };

        if (body.Synonyms?.Count > 0)
            entity.Synonyms = body.Synonyms
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => new DrugSynonym { DrugId = entity.Id, Text = s.Trim() })
                .ToList();

        db.Drugs.Add(entity);
        await db.SaveChangesAsync(ct);

        return Ok(Map(entity));
    }

    // ---------------------------
    // UPDATE
    // ---------------------------
    [Authorize(Roles = "Doctor,Secretary")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<DrugResponse>> Update(Guid id, [FromBody] UpsertDrugRequest body, CancellationToken ct)
    {
        var (ok, err) = Validate(body);
        if (!ok) return BadRequest(err);

        var e = await db.Drugs.Include(d => d.Synonyms).FirstOrDefaultAsync(d => d.Id == id, ct);
        if (e is null) return NotFound();

        e.BrandName = body.BrandName.Trim();
        e.GenericName = body.GenericName.Trim();
        e.Form = (DrugForm)body.Form;
        e.Route = (DrugRoute)body.Route;
        e.StrengthValue = body.StrengthValue;
        e.StrengthUnit = string.IsNullOrWhiteSpace(body.StrengthUnit) ? null : body.StrengthUnit.Trim();
        e.ConcentrationText = string.IsNullOrWhiteSpace(body.ConcentrationText) ? null : body.ConcentrationText.Trim();
        e.RxClass = (RxClass)body.RxClass;
        e.Manufacturer = string.IsNullOrWhiteSpace(body.Manufacturer) ? null : body.Manufacturer.Trim();
        e.Country = string.IsNullOrWhiteSpace(body.Country) ? null : body.Country.Trim();
        e.Barcode = string.IsNullOrWhiteSpace(body.Barcode) ? null : body.Barcode.Trim();
        e.Tags = string.IsNullOrWhiteSpace(body.Tags) ? null : body.Tags.Trim();
        e.IsActive = body.IsActive;
        e.UpdatedAtUtc = DateTime.UtcNow;

        // replace synonyms
        var newSyns = (body.Synonyms ?? []).Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).Distinct().ToHashSet(StringComparer.OrdinalIgnoreCase);
        var toRemove = e.Synonyms.Where(s => !newSyns.Contains(s.Text)).ToList();
        foreach (var r in toRemove) db.DrugSynonyms.Remove(r);
        var existing = e.Synonyms.Select(s => s.Text).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var toAdd = newSyns.Where(s => !existing.Contains(s)).Select(s => new DrugSynonym { DrugId = e.Id, Text = s });
        foreach (var a in toAdd) e.Synonyms.Add(a);

        await db.SaveChangesAsync(ct);
        return Ok(Map(e));
    }

    // ---------------------------
    // GET by Id
    // ---------------------------
    [Authorize(Roles = "Doctor,Secretary,Patient")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DrugResponse>> GetById(Guid id, CancellationToken ct)
    {
        var e = await db.Drugs.AsNoTracking().Include(d => d.Synonyms).FirstOrDefaultAsync(d => d.Id == id, ct);
        if (e is null) return NotFound();
        return Ok(Map(e));
    }

    // ---------------------------
    // SEARCH (full page)
    // ---------------------------
    [Authorize(Roles = "Doctor,Secretary,Patient")]
    [HttpGet("search")]
    public async Task<ActionResult<DrugSearchResult>> Search(
        [FromQuery] string? q,
        [FromQuery] int? form,
        [FromQuery] int? route,
        [FromQuery] int? rxClass,
        [FromQuery] bool? activeOnly,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0 || pageSize > 100) pageSize = 20;

        var query = db.Drugs.AsNoTracking().Include(d => d.Synonyms).AsQueryable();

        if (activeOnly is true) query = query.Where(d => d.IsActive);
        if (form.HasValue) query = query.Where(d => d.Form == (DrugForm)form.Value);
        if (route.HasValue) query = query.Where(d => d.Route == (DrugRoute)route.Value);
        if (rxClass.HasValue) query = query.Where(d => d.RxClass == (RxClass)rxClass.Value);

        if (!string.IsNullOrWhiteSpace(q))
        {
            var s = q.Trim().ToLowerInvariant();
            query = query.Where(d =>
                d.BrandName.ToLower().Contains(s) ||
                d.GenericName.ToLower().Contains(s) ||
                (d.Tags != null && d.Tags.ToLower().Contains(s)) ||
                d.Synonyms.Any(x => x.Text.ToLower().Contains(s)));
        }

        // Rank brand match first, then generic, then strength/form tie-break
        query = query.OrderBy(d => d.BrandName)
                     .ThenBy(d => d.GenericName)
                     .ThenBy(d => d.StrengthValue);

        var total = await query.CountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(d => Map(d))
            .ToListAsync(ct);

        return Ok(new DrugSearchResult { Total = total, Page = page, PageSize = pageSize, Items = items });
    }

    // ---------------------------
    // AUTOCOMPLETE (fast, small payload)
    // ---------------------------
    [Authorize(Roles = "Doctor,Secretary,Patient")]
    [HttpGet("autocomplete")]
    public async Task<ActionResult<List<DrugAutocompleteItem>>> Autocomplete(
        [FromQuery] string q,
        [FromQuery] int limit = 10,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(q)) return Ok(new List<DrugAutocompleteItem>());
        if (limit <= 0 || limit > 30) limit = 10;

        var s = q.Trim().ToLowerInvariant();

        var list = await db.Drugs.AsNoTracking()
            .Include(d => d.Synonyms)
            .Where(d => d.IsActive &&
                        (d.BrandName.ToLower().Contains(s) ||
                         d.GenericName.ToLower().Contains(s) ||
                         (d.Tags != null && d.Tags.ToLower().Contains(s)) ||
                         d.Synonyms.Any(x => x.Text.ToLower().Contains(s))))
            .OrderBy(d => d.BrandName)
            .ThenBy(d => d.GenericName)
            .ThenBy(d => d.StrengthValue)
            .Take(limit)
            .Select(d => new DrugAutocompleteItem
            {
                Id = d.Id,
                GenericName = d.GenericName,
                Form = (int)d.Form,
                Route = (int)d.Route,
                Label = BuildLabel(d.BrandName, d.GenericName, d.StrengthValue, d.StrengthUnit, d.ConcentrationText, d.Form)
            })
            .ToListAsync(ct);

        return Ok(list);
    }

    // ---------------------------
    // MOST-USED (last 90 days)
    // ---------------------------
    [Authorize(Roles = "Doctor,Secretary")]
    [HttpGet("most-used")]
    public async Task<ActionResult<List<DrugAutocompleteItem>>> MostUsed(
        [FromQuery] int days = 90, [FromQuery] int limit = 15, CancellationToken ct = default)
    {
        if (days <= 0 || days > 365) days = 90;
        if (limit <= 0 || limit > 50) limit = 15;

        var since = DateTime.UtcNow.AddDays(-days);

        // If PrescriptionItem.DrugId is not set, you can fallback to grouping by DrugName; here we rely on DrugId.
        var query =
            from pi in db.PrescriptionItems.AsNoTracking()
            join d in db.Drugs.AsNoTracking() on pi.DrugId equals d.Id
            where d.IsActive && pi.DrugId != null
            join p in db.Prescriptions.AsNoTracking() on pi.PrescriptionId equals p.Id
            where p.IssuedAtUtc >= since
            group new { pi, d } by d into g
            orderby g.Count() descending
            select new
            {
                Drug = g.Key,
                Count = g.Count()
            };

        var rows = await query.Take(limit).ToListAsync(ct);

        var result = rows.Select(r => new DrugAutocompleteItem
        {
            Id = r.Drug.Id,
            GenericName = r.Drug.GenericName,
            Form = (int)r.Drug.Form,
            Route = (int)r.Drug.Route,
            Label = BuildLabel(r.Drug.BrandName, r.Drug.GenericName, r.Drug.StrengthValue, r.Drug.StrengthUnit, r.Drug.ConcentrationText, r.Drug.Form)
        }).ToList();

        return Ok(result);
    }

    // ---------------------------
    // Helpers
    // ---------------------------
    private static (bool ok, string? err) Validate(UpsertDrugRequest b)
    {
        if (string.IsNullOrWhiteSpace(b.BrandName)) return (false, "BrandName required.");
        if (string.IsNullOrWhiteSpace(b.GenericName)) return (false, "GenericName required.");
        if (!Enum.IsDefined(typeof(DrugForm), b.Form)) return (false, "Invalid Form.");
        if (!Enum.IsDefined(typeof(DrugRoute), b.Route)) return (false, "Invalid Route.");
        if (!Enum.IsDefined(typeof(RxClass), b.RxClass)) return (false, "Invalid RxClass.");
        if (b.StrengthValue is < 0) return (false, "StrengthValue cannot be negative.");
        return (true, null);
    }

    private static DrugResponse Map(Drug d) => new()
    {
        Id = d.Id,
        BrandName = d.BrandName,
        GenericName = d.GenericName,
        Form = (int)d.Form,
        Route = (int)d.Route,
        StrengthValue = d.StrengthValue,
        StrengthUnit = d.StrengthUnit,
        ConcentrationText = d.ConcentrationText,
        RxClass = (int)d.RxClass,
        Manufacturer = d.Manufacturer,
        Country = d.Country,
        Barcode = d.Barcode,
        Tags = d.Tags,
        Synonyms = d.Synonyms.Select(s => s.Text).ToList(),
        IsActive = d.IsActive
    };

    private static string BuildLabel(string brand, string generic, decimal? val, string? unit, string? conc, DrugForm form)
    {
        var strength = conc ?? (val.HasValue ? $"{val.Value:g}{(string.IsNullOrWhiteSpace(unit) ? "" : " " + unit)}" : null);
        var formTxt = form.ToString().ToLowerInvariant();
        return strength is null
            ? $"{brand} ({generic}) {formTxt}"
            : $"{brand} ({generic}) {strength} {formTxt}";
    }

    private (Guid? userId, string role) GetUserIdAndRole()
    {
        var role = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role") ?? User.FindFirstValue("roles") ?? "";
        var sub  = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        Guid? uid = null;
        if (Guid.TryParse(sub, out var parsed)) uid = parsed;
        return (uid, role);
    }
}
