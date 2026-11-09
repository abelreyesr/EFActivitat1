using Activitat1.Models;
using Microsoft.EntityFrameworkCore;

namespace Activitat1.Data;

public class RefugiContext : DbContext
{
    public RefugiContext(){}
    
    public DbSet<Animal> Animals { get; set; }
    public DbSet<Xip> Xips { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlite("Data Source=refugi.db").UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>()
            .HasOne(a => a.Xip)
            .WithOne(x => x.Animal)
            .HasForeignKey<Animal>(a => a.XipID)
            .IsRequired();

        modelBuilder.Entity<Xip>()
            .HasIndex(x => x.Codi);
    }
}