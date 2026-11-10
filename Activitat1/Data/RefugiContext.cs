using Activitat1.Models;
using Microsoft.EntityFrameworkCore;

namespace Activitat1.Data;

public class RefugiContext : DbContext
{
    public RefugiContext(){}
    
    public DbSet<Animal> Animals { get; set; }
    public DbSet<Xip> Xips { get; set; }
    public DbSet<Adoptant> Adoptants { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlite("Data Source=refugi.db").UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // relacio 1-1 animal-xip
        modelBuilder.Entity<Animal>()
            .HasOne(a => a.Xip)
            .WithOne(x => x.Animal)
            .HasForeignKey<Animal>(a => a.XipID)
            .IsRequired();

        // relacio 1-N adoptant-animals
        modelBuilder.Entity<Animal>()
            .HasOne(a => a.Adoptant)
            .WithMany(d => d.Animals)
            .HasForeignKey("AdoptantID")
            .IsRequired(false);

        // enums en strings en comptes de id
        modelBuilder.Entity<Animal>()
            .Property(a => a.Especie)
            .HasConversion<string>();
    }
}