using API.Models.Entities;

namespace API.Models.DTO;

public class OrdineDto
{
    public int IdOrdine { get; set; }
    public decimal TotaleOrdine { get; set; } = 0;
    public DateTime DataOrdine { get; set; }
    public string Stato { get; set; } = "";
    public string MetodoPagamento { get; set; } = "";
    public int ClienteId { get; set; }
    public string NomeCliente { get; set; } = "";


    public static OrdineDto FromEntity(Ordine ordine)
    {
        return new OrdineDto
        {
            IdOrdine = ordine.IdOrdine,
            TotaleOrdine = ordine.TotaleOrdine,
            DataOrdine = ordine.DataOrdine,
            Stato = ordine.Stato,
            MetodoPagamento = ordine.MetodoPagamento,
            ClienteId = ordine.ClienteId,
            NomeCliente = (ordine.Cliente.Nome + " " + ordine.Cliente.Cognome) ?? ""
        };
    }   
}
