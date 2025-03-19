using System.ComponentModel.DataAnnotations;

namespace API.Models.Entities;

public class UtenteBloccato
{
    [Key]
    public int Id { get; set; }
    public int IdCliente { get; set; }
    public string Email { get; set; } = "";
    public string NomeCompleto { get; set; } = "";
    public string? Ip { get; set; }
    public DateTime DataBan { get; set; } = DateTime.UtcNow;

}
