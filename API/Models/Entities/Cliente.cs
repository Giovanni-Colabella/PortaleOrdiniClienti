using API.Models.Entities;
using API.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Index("Email", IsUnique = true)]
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Cognome { get; set; } = "";
        public string Email { get; set; } = "";
        public string NumeroTelefono { get; set; } = "";
        public Indirizzo Indirizzo { get; set; } = new Indirizzo("", "", "");
        public string Status { get; set; } = "";
        public DateTime DataIscrizione { get; set; }

        public List<Ordine> Ordini { get; set; } = new List<Ordine>();
        public Carrello Carrello { get; set; }
    }
}
