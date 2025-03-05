using API.Models.ValueObjects;

namespace API.Models;

public record ClienteDto(
    int Id,
    string Nome,
    string Cognome,
    string Email,
    string NumeroTelefono,
    Indirizzo Indirizzo,
    string Status,
    DateTime DataIscrizione
);
