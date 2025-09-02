using Api.Endpoints.Models.Apointments;
using Api.Endpoints.Models.Appointments;
using Api.Endpoints.Models.Clinic;
using Api.Endpoints.Models.Doctores;
using Api.Endpoints.Models.MediaclService;
using Api.Endpoints.Models.Prescriptions;
using Api.Endpoints.Models.Schedule;
using Api.Endpoints.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MedicalRecord = Api.Endpoints.Models.Prescriptions.MedicalRecord;

namespace Api.Endpoints.Context;

public class AppDbContext (
    DbContextOptions<AppDbContext> options
) : IdentityDbContext<AppUser, IdentityRole<long>, long>(options)
{
    public DbSet<ClinicSettings> ClinicSettings => Set<ClinicSettings>();
    public DbSet<MedicalService> MedicalServices => Set<MedicalService>();
    public DbSet<WorkSchedule> WorkSchedules => Set<WorkSchedule>();
    public DbSet<SpecialDateOverride> SpecialDateOverrides => Set<SpecialDateOverride>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<DoctorProfile> DoctorProfiles => Set<DoctorProfile>();

    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<PrescriptionItem> PrescriptionItems => Set<PrescriptionItem>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        builder.Entity<AppUser>(b =>
        {
            b.HasIndex(u => u.NormalizedUserName).IsUnique();
            b.Property(u => u.PhoneNumber).HasMaxLength(32);
            b.Property(u => u.FullName).HasMaxLength(128);
        });

        builder.Entity<IdentityRole<long>>(b =>
        {
            b.HasIndex(r => r.NormalizedName).IsUnique();
        });


        builder.Entity<ClinicSettings>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.Address).HasMaxLength(500).IsRequired();
            e.Property(x => x.PhoneNumber).HasMaxLength(50).IsRequired();
        });


        builder.Entity<MedicalService>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.Title).HasMaxLength(200).IsRequired();
            e.OwnsOne(x => x.Price,
                p =>
                {
                    p.Property(m => m.Amount).HasColumnType("decimal(18,2)");
                    p.Property(m => m.Currency).HasMaxLength(10);
                });
        });


        var timeOnlyToTimeSpan = new ValueConverter<TimeOnly, TimeSpan>(
            t => t.ToTimeSpan(),
            ts => TimeOnly.FromTimeSpan(ts)
        );

        builder.Entity<WorkSchedule>(e =>
        {
            e.HasKey(x => x.Id);
            e.OwnsMany(x => x.Days,
                d =>
                {
                    d.WithOwner();
                    d.Property(x => x.Day).HasConversion<int>();
                    d.OwnsMany(x => x.Intervals,
                        tr =>
                        {
                            tr.WithOwner();
                            tr.Property(x => x.From).HasConversion(timeOnlyToTimeSpan);
                            tr.Property(x => x.To).HasConversion(timeOnlyToTimeSpan);
                        });

                    d.OwnsMany(x => x.Breaks,
                        tr =>
                        {
                            tr.WithOwner();
                            tr.Property(x => x.From).HasConversion(timeOnlyToTimeSpan);
                            tr.Property(x => x.To).HasConversion(timeOnlyToTimeSpan);
                        });
                });

            e.HasMany(x => x.Overrides).WithOne().OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<SpecialDateOverride>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Date);
            e.OwnsMany(x => x.Intervals,
                tr =>
                {
                    tr.WithOwner();
                    tr.Property(x => x.From).HasConversion(timeOnlyToTimeSpan);
                    tr.Property(x => x.To).HasConversion(timeOnlyToTimeSpan);
                });
        });

        builder.Entity<MedicalRecord>(cfg =>
        {
            cfg.HasKey(x => x.Id);
            cfg.Property(x => x.Notes).IsRequired().HasMaxLength(4000);
            cfg.Property(x => x.Diagnosis).HasMaxLength(2000);
            cfg.Property(x => x.TreatmentPlan).HasMaxLength(2000);

            cfg.HasIndex(x => x.AppointmentId);
            cfg.HasOne<Appointment>() // assuming you want FK navigation
                .WithMany()
                .HasForeignKey(x => x.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Prescription>(cfg =>
        {
            cfg.HasKey(x => x.Id);
            cfg.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey(i => i.PrescriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            cfg.Property(x => x.Status).HasConversion<int>();
            cfg.Property(x => x.IssueMethod).HasConversion<int>();

            cfg.HasIndex(x => new
            {
                x.PatientUserId,
                x.IssuedAtUtc
            });
        });
    }
}