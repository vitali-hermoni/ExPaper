using ExPaper.Auth.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExPaper.Auth.API.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }


    public DbSet<AppUser> AppUser { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);

    }
}
