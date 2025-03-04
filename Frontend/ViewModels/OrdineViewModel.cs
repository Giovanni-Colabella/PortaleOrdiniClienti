namespace Frontend.ViewModels;

public class OrdineViewModel
{
    public int IdOrdine { get; set; }
    public decimal TotaleOrdine { get; set; } = 0;
    public DateTime DataOrdine { get; set; }
    public string Stato { get; set; } = "";
    public string MetodoPagamento { get; set; } = "";
    public int ClienteId { get; set; }
    public string NomeCliente { get; set; } = "";
}
