using AppointmentPlanner.Shared.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppointmentPlanner.Data.Context;

public class AppDbContext : IdentityDbContext<User , IdentityRole<long>, long>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

}