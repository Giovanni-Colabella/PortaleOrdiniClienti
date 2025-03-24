using System.ComponentModel.DataAnnotations;

namespace API.Models.Entities;

public class Carrello
{
    [Key]
    public int Id { get; set; }

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    public List<CarrelloProdotto> CarrelloProdotti { get; set; }
}
