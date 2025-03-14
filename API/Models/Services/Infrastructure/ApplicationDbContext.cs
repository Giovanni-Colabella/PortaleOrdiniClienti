using API.Models;
using API.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            #region Entità Cliente
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
            #endregion

            #region Entità Ordine 
            modelBuilder.Entity<Ordine>()
                .HasOne(o => o.Cliente)
                .WithMany(c => c.Ordini)
                .HasForeignKey(o => o.ClienteId)
                .OnDelete(DeleteBehavior.Cascade); // Se un cliente viene cancellato, cancella anche i suoi ordini
            #endregion

            #region Entità DettaglioOrdine
            // Non strettamente necessarie dato che ho definito già tutti i comportamenti nell'entità 
            modelBuilder.Entity<DettaglioOrdine>()
                .HasOne(d => d.Ordine)
                .WithMany( o => o.DettagliOrdini)
                .HasForeignKey(d => d.OrdineId);

            // Non strettamente necessarie dato che ho definito già tutti i comportamenti nell'entità 
            modelBuilder.Entity<DettaglioOrdine>()
                .HasOne(d => d.Prodotto)
                .WithMany(d => d.DettagliOrdini)
                .HasForeignKey(op => op.ProdottoId);
            #endregion

        }

        // Tabelle 
        #region  Tabelle 
        public DbSet<Cliente> Clienti { get; set; } // Tabella Clienti
        public DbSet<Ordine> Ordini { get; set; } // Tabella Ordini
        public DbSet<Prodotto> Prodotti { get; set; } // Tabella Prodotti
        public DbSet<DettaglioOrdine> DettagliOrdini { get; set; } // Tabella intermedia DettagliOrdini 
        #endregion
    }
}
