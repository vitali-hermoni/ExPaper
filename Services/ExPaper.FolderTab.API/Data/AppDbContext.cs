using ExPaper.FolderTab.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ExPaper.FolderTab.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}


        public DbSet<FolderTabModel> FolderTab { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
