namespace API.Models.DTO.Mappings;

public static class ClienteMapping
{
    public static Cliente ToEntity(this ClienteDto clienteDto)
    {
        return new Cliente
        {
            Id = clienteDto.Id,
            Nome = clienteDto.Nome,
            Cognome = clienteDto.Cognome,
            Email = clienteDto.Email,
            NumeroTelefono = clienteDto.NumeroTelefono,
            Indirizzo = clienteDto.Indirizzo,
            Status = clienteDto.Status,
            DataIscrizione = clienteDto.DataIscrizione
        };
    }

    public static ClienteDto ToDto(this Cliente cliente)
    {
        return new ClienteDto(
            cliente.Id,
            cliente.Nome,
            cliente.Cognome,
            cliente.Email,
            cliente.NumeroTelefono,
            cliente.Indirizzo,
            cliente.Status,
            cliente.DataIscrizione
        );
    }
}
