using System.ComponentModel.DataAnnotations;

namespace API.Models.Entities;

public class Ordine
{
    [Key]
    public int IdOrdine { get; set; }
    [Required(ErrorMessage = "Il campo 'TotaleOrdine' è obbligatorio")]
    public decimal TotaleOrdine { get; set; } = 0;
    [Required(ErrorMessage = "Il campo 'DataOrdine' è obbligatorio")]
    public DateTime DataOrdine { get; set; }

    [Required(ErrorMessage = "Il campo 'Stato' è obbligatorio")]
    [RegularExpression(@"^(In elaborazione|Spedito|Consegnato|Annullato)$",
        ErrorMessage = "Lo stato dell'ordine deve essere 'In elaborazione', 'Spedito', 'Consegnato' o 'Annullato'")]
    public string Stato { get; set; } = "";

    [Required(ErrorMessage = "Il campo 'MetodoPagamento' è obbligatorio")]
    [RegularExpression(@"^(Carta di credito|PayPal|Bonifico bancario)$",
        ErrorMessage = "Il metodo di pagamento deve essere 'Carta di credito', 'PayPal' o 'Bonifico bancario'")]
    public string MetodoPagamento { get; set; } = "";


    // Foreign key
    public int ClienteId { get; set; }
    // Proprietà di navigazione
    public Cliente Cliente { get; set; } = null!; // Ho usato il null forgiving operator per specificare che sono sicuro che questa var non sarà mai nulla

}
