using ExPaper.Global.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ExPaper.Global.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}


        public DbSet<GlobalSettingModel> GlobalSetting { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
