using System.ComponentModel.DataAnnotations;

namespace API.Models.Entities
{
    public class Prodotto
    {
        [Key]
        public int IdProdotto { get; set; }
        [Required]
        public string NomeProdotto { get; set; } = "";
        [Required]
        public string Categoria { get; set; } = "";
        [Required]
        public decimal Prezzo { get; set; } = 0.00M;
        [Required]
        public int Giacenza { get; set; } = 1;

        // Caricamento Immagine
        [Required]
        public string ImgPath { get; set; } = "";
        // Relazione many-to-many con Ordine 
        public List<DettaglioOrdine> DettagliOrdini { get; set; } = new();
    }
}