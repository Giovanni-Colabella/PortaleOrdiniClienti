using API.Models;
using API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .OwnsOne(c => c.Indirizzo, indirizzo =>
                {

                    // Configurazione delle prop Indirizzo
                    indirizzo.Property(i => i.Via)
                        .HasColumnName("Indirizzo_Via")
                        .IsRequired(); // Proprietà obbligatoria 

                    indirizzo.Property(i => i.Citta)
                        .HasColumnName("Indirizzo_Citta")
                        .IsRequired();

                    indirizzo.Property(i => i.CAP)
                        .HasColumnName("Indirizzo_CAP")
                        .IsRequired();
                });

        }

        // Tabelle 
        public DbSet<Cliente> Clienti { get; set; } // Tabella Clienti
        public DbSet<Ordine> Ordini { get; set; } // Tabella Ordini
    }
}
