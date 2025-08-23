using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext (
    DbContextOptions<AppDbContext> options
) : IdentityDbContext<AppUser, IdentityRole<long>, long>(options)
{
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
    }
}