using ExPaper.Folder.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ExPaper.Folder.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}


        public DbSet<FolderModel> Folder { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
