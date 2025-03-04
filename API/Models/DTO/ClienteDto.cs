using API.Models.ValueObjects;

namespace API.Models;

public class ClienteDto 
{
    public int Id { get;  set; }
    public string Nome { get;  set; } = "";
    public string Cognome { get; set; } = "";
    public string Email { get; set; } = "";
    public string NumeroTelefono { get; set; } = "";
    public Indirizzo Indirizzo { get; set; } = new Indirizzo("", "", "");
    public string Status { get; set; } = "";
    public DateTime DataIscrizione { get; set; }

    public static ClienteDto FromEntity(Cliente cliente)
    {
        return new ClienteDto
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Cognome = cliente.Cognome,
            Email = cliente.Email,
            NumeroTelefono = cliente.NumeroTelefono,
            Indirizzo = new Indirizzo
            (
                cliente.Indirizzo.Via,
                cliente.Indirizzo.Citta,
                cliente.Indirizzo.CAP
            ),
            Status = cliente.Status,
            DataIscrizione = cliente.DataIscrizione
        };
    }

}
