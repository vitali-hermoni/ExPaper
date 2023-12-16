using ExPaper.Organisation.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ExPaper.Organisation.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}


        public DbSet<OrganisationModel> Organisation { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
