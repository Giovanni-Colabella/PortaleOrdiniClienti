namespace API.Models.DTO;

public record OrdineDto (
    int IdOrdine, 
    DateTime DataOrdine, 
    int ClienteId, 
    decimal TotaleOrdine,
    string Stato,
    string MetodoPagamento, 
    string NomeCliente
);