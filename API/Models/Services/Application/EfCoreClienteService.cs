using API.Models.ValueObjects;
using API.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Models.Services.Application
{
    public class EfCoreClienteService : IClienteService
    {
        private readonly ApplicationDbContext _context;
        public EfCoreClienteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateClienteAsync(ClienteDto clienteDto)
        {
            var altroCliente = await _context.Clienti.FirstOrDefaultAsync(c => c.Email == clienteDto.Email);
            if (altroCliente != null)
            {
                return false;
            }

            var cliente = new Cliente
            {
                Nome = clienteDto.Nome,
                Cognome = clienteDto.Cognome,
                Email = clienteDto.Email,
                NumeroTelefono = clienteDto.NumeroTelefono ?? "",
                Indirizzo = new Indirizzo(
                    clienteDto.Indirizzo.Via,
                    clienteDto.Indirizzo.Citta,
                    clienteDto.Indirizzo.CAP
                ),
                Status = clienteDto.Status,
                DataIscrizione = clienteDto.DataIscrizione
            };

            _context.Clienti.Add(cliente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteClienteAsync(int id)
        {
            var cliente = await _context.Clienti.FindAsync(id);
            if (cliente == null)
            {
                return false;
            }

            _context.Clienti.Remove(cliente);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ClienteDto> GetCliente(int id)
        {
            var cliente = await _context.Clienti.FindAsync(id);
            if (cliente == null)
            {
                throw new KeyNotFoundException("Cliente non trovato");
            }

            var clienteDto = ClienteDto.FromEntity(cliente);

            return clienteDto;
        }

        public async Task<List<ClienteDto>> GetClientiAsync()
        {
            // Aspettiamo la query asincrona
            var clienti = await _context.Clienti.OrderByDescending(c => c.Id).ToListAsync();

            // Per ogni cliente andiamo a creare il corrispettivo DTO
            List<ClienteDto> clientiDto = clienti.Select(cliente => 
                ClienteDto.FromEntity(cliente)
            ).ToList();

            // Restituisci la lista di DTO
            return clientiDto;
        }


        public async Task<ClienteDto> UpdateClienteAsync(int id, ClienteDto clienteDto)
        {
            var cliente = await _context.Clienti.FindAsync(id);
            if (cliente == null)
            {
                throw new KeyNotFoundException("Cliente non trovato");
            }

            // Aggiorna le proprietà del cliente
            cliente.Nome = clienteDto.Nome;
            cliente.Cognome = clienteDto.Cognome;
            cliente.Email = clienteDto.Email;
            cliente.NumeroTelefono = clienteDto.NumeroTelefono ?? "";
            cliente.Indirizzo = new Indirizzo
            {
                Via = clienteDto.Indirizzo.Via,
                Citta = clienteDto.Indirizzo.Citta,
                CAP = clienteDto.Indirizzo.CAP
            };
            cliente.Status = clienteDto.Status;
            cliente.DataIscrizione = clienteDto.DataIscrizione;

            // Salva le modifiche nel database
            await _context.SaveChangesAsync();

            // Restituisce il DTO aggiornato
            return ClienteDto.FromEntity(cliente);
        }


        public bool ClienteExists(int id)
        {
            // Controlla se esiste un cliente con l'id specificato
            return _context.Clienti.Any(c => c.Id == id);
        }

    }
}